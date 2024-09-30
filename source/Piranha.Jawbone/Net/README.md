# Piranha.Jawbone.Net

Game dev friendly sockets! Currently, only UDP is supported, but TCP will come.

## Motivation

When C# was born, it was created with the mantra "small heap allocations are free". Most code just freely created strings, arrays, and delegates with no concern for GC pressure. After all, "engineering time is cheaper than compute time", right? Just have your method return a new `byte[]`. It makes the method signature so nice and pretty and easy to understand! Need to process a portion of a string? Just isolate the segment and spin it off into a brand new string! ALLOCATE ALL THE THINGS. It's kind of ironic that 20 years later, even despite the many crazy improvements in computer hardware, we're now realizing two things: allocations are, in fact, _not_ that cheap, and data locality is actually kind of important.

The .NET socket APIs were created in this early era. All the design focus went into develop ergonomics and proper object hierarchies. The type `IPAddress` is, _unfortunately_, a class. That means every instance has 24 bytes of overhead in 64-bit systems (12 bytes in 32-bit systems) just to exist. From there, `IPAddress` serves as a hybrid for IPv4 and IPv6 addresses. Internally, the class only has room for an IPv4 address. So, what if you want an IPv6 address? That's right: it privately allocates an array to hold the rest of the bytes needed for those extra long addresses. And now, what if you want to pair that address with a port? You need the `IPEndPoint` class, which extends the **abstract class** `EndPoint`. So, tracking a single IPv6 endpoint (address and port) is 3 heap allocations.

Also, 'endpoint' is one word. Not sure what happened there. 🙃

Ultimately, the other problem with the included sockets API is that they are too low level. They have all this ceremony around picking socket types, address families, etc. An attempt was made to simpify this with UDP-specific and TCP-specific classes, but it's just more allocations with bloated interfaces that predate `Span<T>`.

## Design

There are two address types: `AddressV4` and `AddressV6`. Both are structs. They are very simple to use.

```csharp
var host = new AddressV4(10, 0, 0, 23);

// Lots of shortcuts.
var localhost = AddressV4.Local;

// IPv6 is a little bigger. It accepts spans.
var v6 = new AddressV6([55, 23, 11, 1, 9, 5, 22, 1, 0, 0, 0, 3, 12, 94, 201, 7]);

// Or parse it. Lots of options.
var parsedV6 = AddressV6.Parse("7f13:22e9::4000:910d", null);
```

When you're ready to pair it with a port, just use `Endpoint<T>` (also a struct).

```csharp
var endpointV4 = new Endpoint<AddressV4>(AddressV4.Local, 5000);
var endpointV6 = new Endpoint<AddressV6>(AddressV6.Local, 5000);

// Lots more shortcuts.
var origin = Endpoint.Create(AddressV4.Local, 5000);

// Or you can use some extensions.
var host = new AddressV4(10, 0, 0, 23);
var endpoint = host.OnPort(5000); // Endpoint<AddressV4>
```

Now you're ready to make a socket! The sockets are clearly split into two kinds: one for IPv4 and one for IPv6. This _constrains_ the interface to work with specific address types: `IUdpSocket<T>`.

```csharp
// Create a socket without binding. Ideal for clients.
using var client = UdpSocketV4.Create();

// Create a socket and listen on port 10215. Ideal for servers.
using var server = UdpSocketV4.BindAnyIp(10215);

// Create an IPv6 socket.
using var clientV6 = UdpSocketV6.Create();

// Create an IPv6 server and allow interop with IPv4!
using var serverV6 = UdpSocketV6.BindAnyIp(allowV4: true);
```

Sending data is very simple. It accepts any `ReadOnlySpan<byte>`.

```csharp
var destination = AddressV4.Local.OnPort(10215);
client.Send("Hello!"u8, destination);
```

Receiving data is only marginally more complex. It lets you specify a timeout. (Simply pick zero if you want a non-blocking call.) From there, you are provided a `UdpReceiveResult<T>` that tells you what happened.

```csharp
var buffer = new byte[2048];
var timeout = TimeSpan.FromSeconds(1);
server.Receive(buffer, timeout, out var result);
result.ThrowOnErrorOrTimeout(); // Or you can check result.State
var n = result.ReceivedByteCount;
var message = buffer.AsSpan(0, n);
Console.WriteLine($"Received {n} bytes from host {result.Origin}.");
```