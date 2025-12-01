#nullable enable

using System.IO;
using System.IO.Compression;

using BizHawk.Common;

namespace BizHawk.Client.Common
{
	/// <summary>
	/// A zip writer that writes to a MemoryStream instead of directly to disk.
	/// This allows serializing the entire save state to memory first, then writing to disk asynchronously.
	/// </summary>
	public class MemoryZipWriter : IZipWriter
	{
		private ZipArchive? _archive;
		private MemoryStream? _ms;
		private Zstd? _zstd;
		private readonly CompressionLevel _level;
		private readonly int _zstdCompressionLevel;

		public MemoryZipWriter(int compressionLevel)
		{
			_ms = new MemoryStream();
			_archive = new(_ms, ZipArchiveMode.Create, leaveOpen: true);
			if (compressionLevel == 0)
				_level = CompressionLevel.NoCompression;
			else if (compressionLevel < 5)
				_level = CompressionLevel.Fastest;
			else
				_level = CompressionLevel.Optimal;

			_zstd = new();
			// compressionLevel ranges from 0 to 9
			// normal compression level range for zstd is 1 to 19
			_zstdCompressionLevel = compressionLevel * 2 + 1;
		}

		public void WriteItem(string name, Action<Stream> callback, bool zstdCompress)
		{
			// don't compress with deflate if we're already compressing with zstd
			// this won't produce meaningful compression, and would just be a timesink
			using var stream = _archive!.CreateEntry(name, zstdCompress ? CompressionLevel.NoCompression : _level).Open();

			if (zstdCompress)
			{
				using var z = _zstd!.CreateZstdCompressionStream(stream, _zstdCompressionLevel);
				callback(z);
			}
			else
			{
				callback(stream);
			}
		}

		/// <summary>
		/// Gets the serialized data as a byte array. The archive must be disposed first.
		/// </summary>
		public byte[] GetData()
		{
			if (_archive != null)
			{
				throw new InvalidOperationException("Archive must be disposed before getting data");
			}
			if (_ms == null)
			{
				throw new InvalidOperationException("Memory stream has already been disposed");
			}
			return _ms.ToArray();
		}

		/// <summary>
		/// Writes the serialized data to a file asynchronously.
		/// The archive must be disposed first.
		/// </summary>
		public async System.Threading.Tasks.Task WriteToFileAsync(string path)
		{
			if (_archive != null)
			{
				throw new InvalidOperationException("Archive must be disposed before writing to file");
			}
			if (_ms == null)
			{
				throw new InvalidOperationException("Memory stream has already been disposed");
			}

			_ms.Position = 0;
			using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 8192, useAsync: true))
			{
				await _ms.CopyToAsync(fs).ConfigureAwait(false);
				await fs.FlushAsync().ConfigureAwait(false);
			}
		}

		public void Dispose()
		{
			_archive?.Dispose();
			_archive = null;
			// Don't dispose _ms yet - we need it for GetData() or WriteToFileAsync()
		}

		/// <summary>
		/// Disposes the memory stream. Call this after you're done with the data.
		/// </summary>
		public void DisposeMemoryStream()
		{
			_ms?.Dispose();
			_ms = null;
			_zstd?.Dispose();
			_zstd = null;
		}
	}
}

