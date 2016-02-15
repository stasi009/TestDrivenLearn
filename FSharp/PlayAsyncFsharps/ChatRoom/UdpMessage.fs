
namespace ChatRoom

open System.Net

type UdpMessage =
    | Received of byte[]* int * IPEndPoint
    | ToSend of byte[] * IPEndPoint