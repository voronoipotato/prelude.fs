(*
The MIT License
prelude.fs - my prelude
Copyright(c) 2018 cannorin
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*)

// this file is automatically generated by build.fsx.
// do not edit this directly.

[<AutoOpen>]
module internal Prelude

// from: ToplevelOperators.fs
[<AutoOpen>]
module ToplevelOperators =
  open System
  let inline to_s x = x.ToString()

  let inline (?|) opt df = defaultArg opt df

  let inline (!!) (x: Lazy<'a>) = x.Value

  let inline undefined (x: 'a) : 'b = NotImplementedException(to_s x) |> raise

  let inline reraise' ex = System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(ex).Throw(); failwith "impossible"

  let inline private ccl (fc: ConsoleColor) =
    Console.ForegroundColor <- fc;
    { new IDisposable with
        member x.Dispose() = Console.ResetColor() }

  let inline cprintf color format =
    Printf.kprintf (fun s -> use c = ccl color in printf "%s" s) format

  let inline cprintfn color format =
    Printf.kprintf (fun s -> use c = ccl color in printfn "%s" s) format

  let inline dispose (disp: #System.IDisposable) =
    match disp with null -> () | x -> x.Dispose()

  let inline succ (n: ^number) =
    n + LanguagePrimitives.GenericOne< ^number >

  let inline pred (n: ^number) =
    n - LanguagePrimitives.GenericOne< ^number >

// from: Interop.fs
open System

module Func =
  let inline ofFSharp0 f = new Func<_>(f)
  let inline ofFSharp1 f = new Func<_, _>(f)
  let inline ofFSharp2 f = new Func<_, _, _>(f)
  let inline ofFSharp3 f = new Func<_, _, _, _>(f)
  let inline toFSharp0 (f: Func<_>) () = f.Invoke()
  let inline toFSharp1 (f: Func<_, _>) x = f.Invoke(x)
  let inline toFSharp2 (f: Func<_, _, _>) x y = f.Invoke(x, y)
  let inline toFSharp3 (f: Func<_, _, _, _>) x y z = f.Invoke(x, y, z)

module Action =
  let inline ofFSharp0 a = new Action(a)
  let inline ofFSharp1 a = new Action<_>(a)
  let inline ofFSharp2 a = new Action<_, _>(a)
  let inline ofFSharp3 a = new Action<_, _, _>(a)
  let inline toFSharp0 (f: Action) () = f.Invoke()
  let inline toFSharp1 (f: Action<_>) x = f.Invoke(x)
  let inline toFSharp2 (f: Action<_, _>) x y = f.Invoke(x, y)
  let inline toFSharp3 (f: Action<_, _, _>) x y z = f.Invoke(x, y, z)

module Flag =
  let inline combine (xs: ^flag seq) : ^flag
    when ^flag: enum<int> =
      xs |> Seq.fold (|||) (Unchecked.defaultof< ^flag >)
  let inline contains (x: ^flag) (flags: ^flag) : bool
    when ^flag: enum<int> =
      (x &&& flags) = x 

module Number =
  open System.Globalization

  let inline tryParse< ^T when ^T: (static member TryParse: string -> ^T byref -> bool) > str : ^T option =
    let mutable ret = Unchecked.defaultof<_> in
    if (^T: (static member TryParse: string -> ^T byref -> bool) (str, &ret)) then
      Some ret
    else
      None

  let inline tryParseWith< ^T when ^T: (static member TryParse: string -> NumberStyles -> IFormatProvider -> ^T byref -> bool) > str styleo formato : ^T option =
    let mutable ret = Unchecked.defaultof<_> in
    let style = styleo ?| NumberStyles.None in
    let format = formato ?| CultureInfo.InvariantCulture in
    if (^T: (static member TryParse: string -> NumberStyles -> IFormatProvider -> ^T byref -> bool) (str, style, format, &ret)) then
      Some ret
    else
      None

// from: Patterns.fs
[<AutoOpen>]
module Patterns =
  open System.Text.RegularExpressions

  let (|Regex|_|) pattern input =
    let m = Regex.Match(input, pattern)
    if m.Success then Some(List.tail [ for g in m.Groups -> g.Value ])
    else None

  let (|DefaultValue|) dv x =
    match x with
      | Some v -> v
      | None -> dv
  
  [<AutoOpen>]
  module Kvp =
    open System.Collections.Generic
    type kvp<'a, 'b> = KeyValuePair<'a, 'b>
    let inline KVP (a, b) = kvp(a, b)
    let (|KVP|) (x: kvp<_, _>) = (x.Key, x.Value)

  [<AutoOpen>]
  module Nat =
    type nat = uint32
    let inline S i = i + 1u
    [<Literal>]
    let Z = 0u
    let (|S|Z|) i = if i = 0u then Z else S (i-1u)

// from: String.fs
module String =
  open System
  open System.Text
  open System.Globalization

  let inline startsWith (s: ^a) (str: ^String) : bool = (^String: (member StartsWith: ^a -> bool) str, s)

  let inline endsWith (s: ^a) (str: ^String) : bool = (^String: (member EndsWith: ^a -> bool) str, s)

  let inline contains (s: ^a) (str: ^String) : bool = (^String: (member IndexOf: ^a -> int) str, s) <> -1

  let inline findIndex (q: ^T) (str: ^String) = 
    (^String: (member IndexOf: ^T -> int) (str, q))

  let inline findIndexAfter (q: ^T) i (str: ^String) = 
    (^String: (member IndexOf: ^T -> int -> int) (str, q, i))

  let inline findLastIndex (q: ^T) (str: ^String) = 
    (^String: (member LastIndexOf: ^T -> int) (str, q))

  let inline findLastIndexAfter (q: ^T) i (str: ^String) = 
    (^String: (member LastIndexOf: ^T -> int -> int) (str, q, i))

  let inline insertAt s i (str: string) = str.Insert(i, s)

  let inline removeAfter i (str: string) = str.Remove i

  let inline remove startIndex endIndex (str: string) = str.Remove(startIndex, endIndex)

  let inline substringAfter i (str: string) = str.Substring i

  let inline substring startIndex endIndex (str: string) = str.Substring(startIndex, endIndex)

  let inline normalize (nfo: NormalizationForm option) (str: string) = 
    match nfo with Some nf -> str.Normalize nf | None -> str.Normalize()

  let inline toLower (ci: CultureInfo) (str: string) = str.ToLower ci

  let inline toLowerInvariant (str: string) = str.ToLowerInvariant()

  let inline toUpper (ci: CultureInfo) (str: string) = str.ToUpper ci

  let inline toUpperInvariant (str: string) = str.ToUpperInvariant()

  let inline padLeft i (str: string) = str.PadLeft i

  let inline padLeftBy i c (str: string) = str.PadLeft(i, c)
  
  let inline padRight i (str: string) = str.PadRight i

  let inline padRightBy i c (str: string) = str.PadRight(i, c)

  let inline trim (str: string) = str.Trim()

  let inline trimStart (str: string) = str.TrimStart()

  let inline trimEnd (str: string) = str.TrimEnd()

  let inline trimBy (trimChar: char) (str: string) = str.Trim(trimChar)
  
  let inline trimBySeq (trimChars: char seq) (str: string) = str.Trim(trimChars |> Seq.toArray)

  let inline trimStartBy (trimChar: char) (str: string) = str.TrimStart(trimChar)
  
  let inline trimStartBySeq (trimChars: char seq) (str: string) = str.TrimStart(trimChars |> Seq.toArray)

  let inline trimEndBy (trimChar: char) (str: string) = str.TrimEnd(trimChar)
  
  let inline trimEndBySeq (trimChars: char seq) (str: string) = str.TrimEnd(trimChars |> Seq.toArray)

  let inline replace (before: ^T) (after: ^T) (s: ^String) =
    (^String: (member Replace: ^T -> ^T -> ^String) (s, before, after))

  let inline split (sp: ^T) (s: ^String) =
    (^String: (member Split: ^T array -> StringSplitOptions -> ^String array) (s, [|sp|], StringSplitOptions.None))

  let inline splitSeq (sp: ^T seq) (s: ^String) =
    (^String: (member Split: ^T array -> StringSplitOptions -> ^String array) (s, Seq.toArray sp, StringSplitOptions.None))

  let inline removeEmptyEntries (sp: string array) = sp |> Array.filter (String.IsNullOrEmpty >> not)

  let inline toChars (s: string) = s.ToCharArray()

  let inline ofChars (chars: #seq<char>) = System.String.Concat chars

  let inline nth i (str: string) = str.[i]

  let inline rev (str: string) = 
    new String(str.ToCharArray() |> Array.rev)

  let inline private whileBase pred act str =
    if String.IsNullOrEmpty str then
      ""
    else
      let mutable i = 0
      while i < String.length str && str |> nth i |> pred do i <- i + 1 done
      if i = 0 then ""
      else str |> act i

  let inline take i str =
    if i = 0 then ""
    else if i >= String.length str then str
    else removeAfter i str

  let inline skip i str =
    if i = 0 then str
    else if i >= String.length str then ""
    else substringAfter i str

  let inline takeWhile predicate (str: string) =
    whileBase predicate take str

  let inline skipWhile predicate (str: string) =
    whileBase predicate skip str

  let inline build (builder: StringBuilder -> unit) =
    let sb = new StringBuilder()
    builder sb
    sb.ToString()

[<AutoOpen>]
module StringExtensions =
  open System.Text

  type StringBuilder with
    member inline this.printf format =
      Printf.kprintf (fun s -> this.Append s |> ignore) format

    member inline this.printfn format =
      Printf.kprintf (fun s -> this.AppendLine s |> ignore) format

// from: Collections.fs
open System.Collections.Generic

type array2d<'t> = 't[,]
type array3d<'t> = 't[,,]    

module List =
  let inline splitWith predicate xs =
    List.foldBack (fun x state ->
      if predicate x then
        [] :: state
      else
        match state with
        | [] -> [[x]]
        | h :: t -> (x :: h) :: t
    ) xs []

  let inline split separator xs = splitWith ((=) separator) xs

  let inline skipSafe length xs =
    if List.length xs > length then
      List.skip length xs
    else List.empty

  let inline foldi folder state xs =
    List.fold (fun (i, state) x -> (i + 1, folder i state x)) (0, state) xs |> snd

module Seq =
  let inline splitWith predicate xs =
    let i = ref 1
    xs |> Seq.groupBy (fun x -> if predicate x then incr i; 0 else !i)
       |> Seq.filter (fst >> ((<>) 0))
       |> Seq.map snd
  
  let inline split separator xs = splitWith ((=) separator) xs

  let inline skipSafe length xs = 
    xs |> Seq.indexed
       |> Seq.skipWhile (fst >> ((>) length))
       |> Seq.map snd
  
  let inline foldi folder state xs =
    Seq.fold (fun (i, state) x -> (i + 1, folder i state x)) (0, state) xs |> snd

module Array =
  let inline skipSafe length xs =
    if Array.length xs > length then
      Array.skip length xs
    else Array.empty

  let inline foldi folder state xs =
    Array.fold (fun (i, state) x -> (i + 1, folder i state x)) (0, state) xs |> snd

module Map =
  open FSharp.Collections
  let inline choose c m =
    m |> Map.fold (
      fun newMap k v ->
        match c k v with
          | Some x -> newMap |> Map.add k x
          | None   -> newMap
    ) Map.empty

  /// Appends two maps. If there is a duplicate key,
  /// the value in the latter map (`m2`) will be used.
  let inline append m1 m2 =
    Map.fold (fun m k v -> Map.add k v m) m1 m2

  /// Concats multiple maps. If there is a duplicate key,
  /// the value in the last map containing that key will be used.
  let inline concat ms =
    ms |> Seq.fold (fun state m -> append state m) Map.empty

  /// Merges two maps. If there is a duplicate key, the `merger` function
  /// will be called: the first parameter is the key, the second is the value
  /// found in the formar map `m1`, and the third is the one found in `m2`.
  let inline merge merger m1 m2 =
    Map.fold (fun m k v1 -> 
      match m |> Map.tryFind k with
        | Some v2 -> Map.add k (merger k v1 v2) m
        | None -> Map.add k v1 m
      ) m1 m2
  
  /// Merges multiple maps. If there is a duplicate key, the `merger` function
  /// will be called: the first parameter is the key, the second is the value
  /// already found in the earlier maps, and the third is the value newly found.
  let inline mergeMany merger ms =
    ms |> Seq.fold (fun state m -> merge merger state m) Map.empty

type dict<'a, 'b> = IDictionary<'a, 'b>

module Dict =
  let inline empty<'a, 'b when 'a: comparison> = Map.empty :> dict<'a, 'b>
  let inline map f (d: #dict<_, _>) =
    dict <| seq {
      for KVP(k, v) in d do
        yield k, f k v
    }
  let inline filter p (d: #dict<_, _>) =
    dict <| seq {
      for KVP(k, v) in d do
        if p k v then yield k,v
    }
  let inline choose c (d: #dict<_, _>) =
    dict <| seq {
      for KVP(k, v) in d do
        match c k v with
          | Some x -> yield k, x
          | None -> ()
    }
  let inline fold f init (d: #dict<_, _>) =
    Seq.fold (fun state (KVP(k, v)) -> f state k v) init d
  let inline count (xs: #dict<_, _>) = xs.Count
  let inline exists pred (xs: #dict<_, _>) =
    xs :> seq<_> |> Seq.exists (function KVP(k, v) -> pred k v)
  let inline containsKey x (xs: #dict<_, _>) = xs.ContainsKey x
  let inline find key (xs: #dict<_, _>) = xs.[key]
  let inline tryFind key (xs: #dict<_, _>) =
    if xs.ContainsKey key then xs.[key] |> Some else None
  let inline toMap (xs: #dict<_, _>) =
    let mutable m = Map.empty
    for KVP(k, v) in xs do
      m <- m |> Map.add k v
    m
  let inline toMutable (xs: #dict<'a, 'b>) = new Dictionary<'a, 'b>(xs :> IDictionary<_, _>)
  let inline toSeq (xs: #dict<_, _>) = xs :> seq<kvp<_, _>>

// from: DataTypes.fs
module Lazy =
  let inline run (x: Lazy<_>) = x.Value

  let inline force (x: Lazy<_>) = x.Force()
  
  let inline bind (f: 'a -> Lazy<'b>) (x: Lazy<'a>) : Lazy<'b> =
    Lazy<_>.Create (fun () -> x |> force |> f |> force)

  let inline returnValue x =
    Lazy<_>.CreateFromValue x

  let inline map (f: 'a -> 'b) (x: Lazy<'a>) =
    lazy (f x.Value)

  let inline flatten (x: Lazy<Lazy<'a>>) = lazy (!!(!!x))

module Tuple =
  let inline map2 f g (x, y) = (f x, g y)
  let inline map3 f g h (x, y, z) = (f x, g y, h z)
  let inline map4 f g h i (x, y, z, w) = (f x, g y, h z, i w)
  let inline map5 f g h i j (x, y, z, w, v) = (f x, g y, h z, i w, j v)

module Result =
  let inline bimap f g res =
    match res with
      | Ok x -> Ok (f x)
      | Error e -> Error (g e)

  let inline toOption res =
    match res with
      | Ok x -> Some x
      | Error _ -> None
  
  let inline toChoice res =
    match res with
      | Ok x -> Choice1Of2 x
      | Error e -> Choice2Of2 e

  let inline ofOption opt =
    match opt with
      | Some x -> Ok x
      | None -> Error ()

  let inline ofChoice cic =
    match cic with
      | Choice1Of2 x -> Ok x
      | Choice2Of2 e -> Error e

  let inline get res =
    match res with
      | Ok x -> x
      | Error e -> reraise' e

  let inline defaultWith f res =
    match res with
      | Ok x -> x
      | Error e -> f e

  let inline defaultValue y res =
    match res with
      | Ok x -> x
      | Error _ -> y

module Async =
  open System
  open Microsoft.FSharp.Control

  let inline run x = Async.RunSynchronously x
  let inline returnValue x = async { return x }
  let inline bind f m = async { let! x = m in return! f x }

  let timeout (timeout : TimeSpan) a =
    async {
      try
        let! child = Async.StartChild(a, int timeout.TotalMilliseconds) in
        let! result = child in
        return Some result
      with
        | :? TimeoutException -> return None
    }

// from: ComputationExpressions.fs
[<AutoOpen>]
module ComputationExpressions =
  open System.Linq

  (*
    // boilerplate for strict monads to add delay/try

    member inline this.Delay f = f
    member inline this.Undelay f = f()
    member inline this.TryWith (f, h) = try f() with exn -> h exn
    member inline this.TryFinally (f, h) = try f() finally h()
    

    // boilerplate for any monad to add for/while
    
    member inline this.Zero () = this.Return ()
    member inline this.Using (disp: #System.IDisposable, m) =
      this.TryFinally(
        this.Delay(fun () -> m disp),
        fun () -> dispose disp
      )
    member inline this.Combine (a, b) = this.Bind (a, fun () -> b)
    member inline this.While (cond, m) =
      let rec loop cond m =
        if cond () then this.Combine(this.Undelay m, loop cond m)
        else this.Zero ()
      loop cond m
    member inline this.For (xs: #seq<_>, exec) =
      this.Using(
        (xs :> seq<_>).GetEnumerator(),
        fun en ->
          this.While(
            en.MoveNext,
            this.Delay(fun () -> exec en.Current))
      )
  *)

  type OptionBuilder() =
    member inline this.Bind(m, f) = Option.bind f m
    member inline this.Return x = Some x
    member inline this.ReturnFrom x = x
    
    member inline this.Delay f = f
    member inline this.Undelay f = f()
    member inline this.TryWith (f, h) = try f() with exn -> h exn
    member inline this.TryFinally (f, h) = try f() finally h()

    member inline this.Zero () = this.Return ()
    member inline this.Using (disp: #System.IDisposable, m) =
      this.TryFinally(
        this.Delay(fun () -> m disp),
        fun () -> dispose disp
      )
    member inline this.Combine (a, b) = this.Bind (a, fun () -> b)
    member inline this.While (cond, m) =
      let rec loop cond m =
        if cond () then this.Combine(this.Undelay m, loop cond m)
        else this.Zero ()
      loop cond m
    member inline this.For (xs: #seq<_>, exec) =
      this.Using(
        (xs :> seq<_>).GetEnumerator(),
        fun en ->
          this.While(
            en.MoveNext,
            this.Delay(fun () -> exec en.Current))
      )
  
  type ResultBuilder() =
    member inline this.Bind(m, f) = Result.bind f m
    member inline this.Return x = Ok x
    member inline this.ReturnFrom x = x
    
    member inline this.Delay f = f
    member inline this.Undelay f = f()
    member inline this.TryWith (f, h) = try f() with exn -> h exn
    member inline this.TryFinally (f, h) = try f() finally h()
    
    member inline this.Zero () = this.Return ()
    member inline this.Using (disp: #System.IDisposable, m) =
      this.TryFinally(
        this.Delay(fun () -> m disp),
        fun () -> dispose disp
      )
    member inline this.Combine (a, b) = this.Bind (a, fun () -> b)
    member inline this.While (cond, m) =
      let rec loop cond m =
        if cond () then this.Combine(this.Undelay m, loop cond m)
        else this.Zero ()
      loop cond m
    member inline this.For (xs: #seq<_>, exec) =
      this.Using(
        (xs :> seq<_>).GetEnumerator(),
        fun en ->
          this.While(
            en.MoveNext,
            this.Delay(fun () -> exec en.Current))
      )
  

  type LazyBuilder() =
    member inline this.Bind(m, f) = Lazy.bind f m
    member inline this.Return x = lazy x
    member inline this.ReturnFrom m = m

    member inline this.Delay f = this.Bind(this.Return (), f)
    member inline this.Undelay x = x
    member inline this.TryWith (m, f) =
      lazy (try Lazy.force m with exn -> f exn)
    member inline this.TryFinally (m, f) =
      lazy (try Lazy.force m finally f() )
    
    member inline this.Zero () = this.Return ()
    member inline this.Using (disp: #System.IDisposable, m) =
      this.TryFinally(
        this.Delay(fun () -> m disp),
        fun () -> dispose disp
      )
    member inline this.Combine (a, b) = this.Bind (a, fun () -> b)
    member inline this.While (cond, m) =
      let rec loop cond m =
        if cond () then this.Combine(this.Undelay m, loop cond m)
        else this.Zero ()
      loop cond m
    member inline this.For (xs: #seq<_>, exec) =
      this.Using(
        (xs :> seq<_>).GetEnumerator(),
        fun en ->
          this.While(
            en.MoveNext,
            this.Delay(fun () -> exec en.Current))
      )

  open System.Threading.Tasks
  type AsyncBuilder with
    member inline this.Bind(t:Task<'T>, f:'T -> Async<'R>) : Async<'R> = 
      async.Bind(Async.AwaitTask t, f)
    member inline this.Bind(t:Task, f:unit -> Async<'R>) : Async<'R> = 
      async.Bind(Async.AwaitTask t, f)

  [<RequireQualifiedAccess>]
  module Do =
    let option = OptionBuilder()
    let result = ResultBuilder()
    let lazy'  = LazyBuilder()


// from: IO.fs
open System
open System.IO

module Path =

  let combine x y = Path.Combine(x, y)

  let combineMany xs = Path.Combine <| Seq.toArray xs

  let makeRelativeTo parentDir file =
    let filePath = new Uri(file)
    let path =
      new Uri (
        if (parentDir |> String.endsWith (to_s Path.DirectorySeparatorChar) |> not) then
          sprintf "%s%c" parentDir Path.DirectorySeparatorChar
        else
          parentDir
      )
    Uri.UnescapeDataString(
      path.MakeRelativeUri(filePath)
      |> to_s
      |> String.replace '/' Path.DirectorySeparatorChar)

module File =
  let inline isHidden path =
    File.GetAttributes(path).HasFlag(FileAttributes.Hidden)

module Directory =
  let inline isHidden dir =
    DirectoryInfo(dir).Attributes.HasFlag(FileAttributes.Hidden)
  
  let rec enumerateFilesRecursively includeHidden dir =
    seq {
      for x in Directory.EnumerateFiles dir do
        if includeHidden || not (File.isHidden x) then
          yield x
      for subdir in Directory.EnumerateDirectories dir do
        if includeHidden || not (isHidden subdir) then
          yield! enumerateFilesRecursively includeHidden subdir
    }

// from: Utilities.fs
module Convert =
  let inline hexsToInt (hexs: #seq<char>) =
    let len = Seq.length hexs - 1
    hexs |> Seq.foldi (fun i sum x ->
      let n =
        let n = int x - int '0'
        if n < 10 then n
        else if n < 23 then n - 7
        else n - 44
      sum + n * pown 16 (len - i)) 0

  let inline digitsToInt (digits: #seq<char>) =
    let len = Seq.length digits - 1
    digits |> Seq.foldi (fun i sum x ->
      sum + (int x - int '0') * pown 10 (len - i)) 0

module Shell =
  open System.Diagnostics

  let inline eval cmd args =
    use p = new Process()
    p.EnableRaisingEvents <- false
    p.StartInfo.UseShellExecute <- false
    p.StartInfo.FileName <- cmd
    p.StartInfo.Arguments <- args |> String.concat " "
    p.StartInfo.RedirectStandardInput <- true
    p.StartInfo.RedirectStandardOutput <- true
    p.Start() |> ignore
    p.WaitForExit()
    p.StandardOutput.ReadToEnd()

  let inline evalAsync cmd args =
    async {
      use p = new Process()
      do p.EnableRaisingEvents <- false
      do p.StartInfo.UseShellExecute <- false
      do p.StartInfo.FileName <- cmd
      do p.StartInfo.Arguments <- args |> String.concat " "
      do p.StartInfo.RedirectStandardInput <- true
      do p.StartInfo.RedirectStandardOutput <- true
      do p.Start() |> ignore
      do p.WaitForExit()
      return p.StandardOutput.ReadToEnd()
    }

  let inline pipe cmd args (stdin: string) =
    use p = new Process()
    p.EnableRaisingEvents <- false
    p.StartInfo.UseShellExecute <- false
    p.StartInfo.FileName <- cmd
    p.StartInfo.Arguments <- args |> String.concat " "
    p.StartInfo.RedirectStandardInput <- true
    p.StartInfo.RedirectStandardOutput <- true
    p.Start() |> ignore
    p.StandardInput.WriteLine stdin
    p.WaitForExit()
    p.StandardOutput.ReadToEnd()

  let inline pipeAsync cmd args (stdin: string) =
    async {
      use p = new Process()
      do p.EnableRaisingEvents <- false
      do p.StartInfo.UseShellExecute <- false
      do p.StartInfo.FileName <- cmd
      do p.StartInfo.Arguments <- args |> String.concat " "
      do p.StartInfo.RedirectStandardInput <- true
      do p.StartInfo.RedirectStandardOutput <- true
      do p.Start() |> ignore
      do! p.StandardInput.WriteLineAsync stdin
      do p.WaitForExit()
      return p.StandardOutput.ReadToEnd()
    }

  let inline run cmd args =
    use p = new Process()
    p.EnableRaisingEvents <- false
    p.StartInfo.UseShellExecute <- false
    p.StartInfo.FileName <- cmd
    p.StartInfo.Arguments <- args |> String.concat " "
    p.Start() |> ignore
    p.WaitForExit()
    p.ExitCode

  let inline runAsync cmd args =
    async {
      use p = new Process()
      do p.EnableRaisingEvents <- false
      do p.StartInfo.UseShellExecute <- false
      do p.StartInfo.FileName <- cmd
      do p.StartInfo.Arguments <- args |> String.concat " "
      do p.Start() |> ignore
      do p.WaitForExit() 
      return p.ExitCode
    }

