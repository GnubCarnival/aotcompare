// Decompiled with JetBrains decompiler
// Type: UVAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


using System.IO;
using UnityEngine;

public class UVAnimation
{
  public int curFrame;
  public Vector2[] frames;
  public int loopCycles;
  public bool loopReverse;
  public string name;
  protected int numLoops;
  protected int stepDir = 1;
  public Vector2[] UVDimensions;

  public void BuildFromFile(string path, int index, float uvTime, Texture mainTex)
  {
    if (!File.Exists(path))
    {
      Debug.LogError((object) "wrong ean file path!");
    }
    else
    {
      FileStream fileStream = new FileStream(path, FileMode.Open);
      BinaryReader br = new BinaryReader((Stream) fileStream);
      EanFile eanFile = new EanFile();
      eanFile.Load(br, fileStream);
      fileStream.Close();
      EanAnimation anim = eanFile.Anims[index];
      this.frames = new Vector2[(int) anim.TotalCount];
      this.UVDimensions = new Vector2[(int) anim.TotalCount];
      int tileCount = (int) anim.TileCount;
      int num = ((int) anim.TotalCount + tileCount - 1) / tileCount;
      int index1 = 0;
      int width = mainTex.width;
      int height = mainTex.height;
      for (int index2 = 0; index2 < num; ++index2)
      {
        for (int index3 = 0; index3 < tileCount && index1 < (int) anim.TotalCount; ++index3)
        {
          Vector2 zero = Vector2.zero;
          zero.x = (float) anim.Frames[index1].Width / (float) width;
          zero.y = (float) anim.Frames[index1].Height / (float) height;
          this.frames[index1].x = (float) anim.Frames[index1].X / (float) width;
          this.frames[index1].y = (float) (1.0 - (double) anim.Frames[index1].Y / (double) height);
          this.UVDimensions[index1] = zero;
          this.UVDimensions[index1].y = -this.UVDimensions[index1].y;
          ++index1;
        }
      }
    }
  }

  public Vector2[] BuildUVAnim(
    Vector2 start,
    Vector2 cellSize,
    int cols,
    int rows,
    int totalCells)
  {
    int index1 = 0;
    this.frames = new Vector2[totalCells];
    this.UVDimensions = new Vector2[totalCells];
    this.frames[0] = start;
    for (int index2 = 0; index2 < rows; ++index2)
    {
      for (int index3 = 0; index3 < cols && index1 < totalCells; ++index3)
      {
        this.frames[index1].x = start.x + cellSize.x * (float) index3;
        this.frames[index1].y = start.y - cellSize.y * (float) index2;
        this.UVDimensions[index1] = cellSize;
        this.UVDimensions[index1].y = -this.UVDimensions[index1].y;
        ++index1;
      }
    }
    return this.frames;
  }

  public bool GetNextFrame(ref Vector2 uv, ref Vector2 dm)
  {
    if (this.curFrame + this.stepDir >= this.frames.Length || this.curFrame + this.stepDir < 0)
    {
      if (this.stepDir > 0 && this.loopReverse)
      {
        this.stepDir = -1;
        this.curFrame += this.stepDir;
        uv = this.frames[this.curFrame];
        dm = this.UVDimensions[this.curFrame];
      }
      else
      {
        if (this.numLoops + 1 > this.loopCycles && this.loopCycles != -1)
          return false;
        ++this.numLoops;
        if (this.loopReverse)
        {
          this.stepDir *= -1;
          this.curFrame += this.stepDir;
        }
        else
          this.curFrame = 0;
        uv = this.frames[this.curFrame];
        dm = this.UVDimensions[this.curFrame];
      }
    }
    else
    {
      this.curFrame += this.stepDir;
      uv = this.frames[this.curFrame];
      dm = this.UVDimensions[this.curFrame];
    }
    return true;
  }

  public void PlayInReverse()
  {
    this.stepDir = -1;
    this.curFrame = this.frames.Length - 1;
  }

  public void Reset()
  {
    this.curFrame = 0;
    this.stepDir = 1;
    this.numLoops = 0;
  }

  public void SetAnim(Vector2[] anim) => this.frames = anim;
}
