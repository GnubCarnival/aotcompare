// Decompiled with JetBrains decompiler
// Type: TypedLobby
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null


public class TypedLobby
{
  public static readonly TypedLobby Default = new TypedLobby();
  public string Name;
  public LobbyType Type;

  public TypedLobby()
  {
    this.Name = string.Empty;
    this.Type = LobbyType.Default;
  }

  public TypedLobby(string name, LobbyType type)
  {
    this.Name = name;
    this.Type = type;
  }

  public override string ToString() => string.Format("Lobby '{0}'[{1}]", (object) this.Name, (object) this.Type);

  public bool IsDefault => this.Type == LobbyType.Default && string.IsNullOrEmpty(this.Name);
}
