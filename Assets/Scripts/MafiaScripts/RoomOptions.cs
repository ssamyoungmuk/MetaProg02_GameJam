internal class RoomOptions : Photon.Realtime.RoomOptions
{
	public int MaxPlayers { get; set; }
	public bool IsOpen { get; set; }
}