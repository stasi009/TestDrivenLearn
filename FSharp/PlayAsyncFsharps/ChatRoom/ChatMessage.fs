
namespace ChatRoom

type ChatMessage=
    | Send of string
    | Clear
    | Get of AsyncReplyChannel<string>
