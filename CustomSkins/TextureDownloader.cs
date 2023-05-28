// Decompiled with JetBrains decompiler
// Type: CustomSkins.TextureDownloader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System;
using System.Collections;
using UnityEngine;
using Utility;

namespace CustomSkins
{
  internal class TextureDownloader
  {
    private static readonly string[] ValidHosts = new string[21]
    {
      "i.imgur.com/",
      "imgur.com/",
      "image.ibb.co/",
      "i.reddit.it/",
      "cdn.discordapp.com/attachments/",
      "media.discordapp.net/attachments/",
      "images-ext-2.discordapp.net/external/",
      "i.reddit.it/",
      "gyazo.com/",
      "puu.sh/",
      "i.postimg.cc/",
      "postimg./",
      "deviantart.com/",
      "photobucket.com/",
      "aotcorehome.files.wordpress.com/",
      "s1.ax1x.com/",
      "s27.postimg.io/",
      "1.bp.blogspot.com/",
      "tiebapic.baidu.com/",
      "s25.postimg.gg/",
      "aotcorehome.files.wordpress.com/"
    };
    private static readonly string[] ValidFileEndings = new string[3]
    {
      ".jpg",
      ".png",
      ".jpeg"
    };
    private static readonly string[] URLPrefixes = new string[3]
    {
      "https://",
      "http://",
      "www."
    };
    private const int MaxConcurrentDownloads = 1;
    private static int CurrentConcurrentDownloads = 0;

    public static void ResetConcurrentDownloads() => TextureDownloader.CurrentConcurrentDownloads = 0;

    public static bool ValidTextureURL(string url)
    {
      url = url.ToLower();
      if (url == string.Empty)
        return false;
      if (url == BaseCustomSkinLoader.TransparentURL)
        return true;
      return TextureDownloader.CheckFileEnding(url) && TextureDownloader.CheckValidHost(url);
    }

    private static bool CheckFileEnding(string url)
    {
      foreach (string validFileEnding in TextureDownloader.ValidFileEndings)
      {
        if (url.EndsWith(validFileEnding))
          return true;
      }
      return false;
    }

    private static bool CheckValidHost(string url)
    {
      if (url.StartsWith("file://"))
        return true;
      foreach (string urlPrefix in TextureDownloader.URLPrefixes)
      {
        if (url.StartsWith(urlPrefix))
          url = url.Remove(0, urlPrefix.Length);
      }
      foreach (string validHost in TextureDownloader.ValidHosts)
      {
        if (url.StartsWith(validHost))
          return true;
      }
      return false;
    }

    public static IEnumerator DownloadTexture(
      BaseCustomSkinLoader obj,
      string url,
      bool mipmap,
      int maxSize)
    {
      Texture2D blankTexture = TextureDownloader.CreateBlankTexture(mipmap);
      yield return (object) blankTexture;
      if (TextureDownloader.ValidTextureURL(url))
      {
        while (!TextureDownloader.CanStartTextureDownload())
          yield return (object) blankTexture;
        TextureDownloader.OnStartTextureDownload();
        using (WWW www = new WWW(url))
        {
          yield return (object) www;
          if (www.error != null || www.bytesDownloaded > maxSize)
          {
            TextureDownloader.OnStopTextureDownload();
            yield return (object) blankTexture;
          }
          else
          {
            TextureDownloader.OnStopTextureDownload();
            CoroutineWithData cwd = new CoroutineWithData((MonoBehaviour) obj, TextureDownloader.CreateTextureFromData(obj, www, mipmap));
            yield return (object) cwd.Coroutine;
            yield return cwd.Result;
            cwd = (CoroutineWithData) null;
          }
        }
      }
    }

    private static bool CanStartTextureDownload() => TextureDownloader.CurrentConcurrentDownloads < 1;

    private static void OnStartTextureDownload()
    {
      ++TextureDownloader.CurrentConcurrentDownloads;
      TextureDownloader.CurrentConcurrentDownloads = Math.Min(TextureDownloader.CurrentConcurrentDownloads, 1);
    }

    private static void OnStopTextureDownload()
    {
      --TextureDownloader.CurrentConcurrentDownloads;
      TextureDownloader.CurrentConcurrentDownloads = Math.Max(TextureDownloader.CurrentConcurrentDownloads, 0);
    }

    private static bool IsPowerOfTwo(int num) => num >= 4 && (num & num - 1) == 0;

    private static int GetClosestPowerOfTwo(int num)
    {
      int closestPowerOfTwo = 4;
      num = Math.Min(num, 2047);
      while (closestPowerOfTwo < num)
        closestPowerOfTwo *= 2;
      return closestPowerOfTwo;
    }

    private static Texture2D CreateBlankTexture(bool mipmap, bool compressed = false) => compressed ? new Texture2D(4, 4, (TextureFormat) 12, mipmap) : new Texture2D(4, 4, (TextureFormat) 4, mipmap);

    private static Texture2D DecodeTexture(WWW www, bool mipmap)
    {
      Texture2D blankTexture = TextureDownloader.CreateBlankTexture(mipmap);
      try
      {
        blankTexture.LoadImage(www.bytes);
      }
      catch
      {
        blankTexture = TextureDownloader.CreateBlankTexture(false);
        blankTexture.LoadImage(www.bytes);
      }
      return blankTexture;
    }

    private static IEnumerator CreateTextureFromData(
      BaseCustomSkinLoader obj,
      WWW www,
      bool mipmap)
    {
      int resizedSize = 0;
      Texture2D texture = TextureDownloader.DecodeTexture(www, mipmap);
      yield return (object) obj.StartCoroutine(Util.WaitForFrames(2));
      int width = ((Texture) texture).width;
      int height = ((Texture) texture).height;
      if (!TextureDownloader.IsPowerOfTwo(width))
        resizedSize = TextureDownloader.GetClosestPowerOfTwo(width);
      else if (!TextureDownloader.IsPowerOfTwo(height))
        resizedSize = TextureDownloader.GetClosestPowerOfTwo(height);
      if (resizedSize == 0)
      {
        texture.Compress(true);
        yield return (object) obj.StartCoroutine(Util.WaitForFrames(2));
        yield return (object) texture;
      }
      else
      {
        yield return (object) obj.StartCoroutine(TextureScaler.Scale(texture, resizedSize, resizedSize));
        yield return (object) obj.StartCoroutine(Util.WaitForFrames(2));
        texture.Compress(true);
        yield return (object) obj.StartCoroutine(Util.WaitForFrames(2));
        yield return (object) texture;
      }
    }
  }
}
