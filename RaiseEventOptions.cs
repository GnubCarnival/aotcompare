// Decompiled with JetBrains decompiler
// Type: RaiseEventOptions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


public class RaiseEventOptions
{
  public int CacheSliceIndex;
  public EventCaching CachingOption;
  public static readonly RaiseEventOptions Default = new RaiseEventOptions();
  public bool ForwardToWebhook;
  public byte InterestGroup;
  public ReceiverGroup Receivers;
  public byte SequenceChannel;
  public int[] TargetActors;
}
