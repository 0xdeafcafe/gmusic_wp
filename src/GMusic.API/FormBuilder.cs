﻿/* This class was originally written by taylorfinnell (https://github.com/taylorfinnell/) for 
 * his un-official Google Music API (https://github.com/taylorfinnell/GoogleMusicAPI.NET/).
 * 
 * Modified by Alex Reed (https://github.com/Xerax) for gmusic_wp (https://github.com/Xerax/gmusic_wp)
 * 
 * yolo
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GMusic.API
{
	public class FormBuilder
	{
		readonly string boundary = "----------" + DateTime.Now.Ticks.ToString("x");
		readonly MemoryStream ms;

		public static FormBuilder Empty
		{
			get
			{
				var b = new FormBuilder();
				b.Close();
				return b;
			}
		}

		public String ContentType
		{
			get
			{
				return "multipart/form-data; boundary=" + boundary;
			}
		}

		public FormBuilder()
		{
			ms = new MemoryStream();
		}

		public void AddFields(Dictionary<String, String> fields)
		{
			foreach (var key in fields)
				AddField(key.Key, key.Value);
		}

		public void AddField(String key, String value)
		{
			var sb = new StringBuilder();

			sb.AppendFormat("\r\n--{0}\r\n", boundary);
			sb.AppendFormat("Content-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}", key, value);

			var sbData = Encoding.UTF8.GetBytes(sb.ToString());

			ms.Write(sbData, 0, sbData.Length);
		}

		public void AddFile(String name, String fileName, byte[] file)
		{
			var sb = new StringBuilder();

			sb.AppendFormat("\r\n--{0}\r\n", boundary);
			sb.AppendFormat("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n", name, fileName);

			sb.AppendFormat("Content-Type: {0}\r\n\r\n", "application/octet-stream");

			var sbData = Encoding.UTF8.GetBytes(sb.ToString());
			ms.Write(sbData, 0, sbData.Length);

			ms.Write(file, 0, file.Length);
		}

		public void Close()
		{
			var sb = new StringBuilder();

			sb.AppendLine("\r\n--" + boundary + "--\r\n");

			var sbData = Encoding.UTF8.GetBytes(sb.ToString());
			ms.Write(sbData, 0, sbData.Length);
		}

		public byte[] GetBytes()
		{
			return ms.ToArray();
		}

		public String GetString()
		{
			var bytes = GetBytes();
			return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
		}
	}
}