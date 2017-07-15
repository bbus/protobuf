open System
open Google.Protobuf 
open ProtobufUnittest
open ProtobufUnittestImport

let inline (+=) (rf: Collections.RepeatedField<'A>) (xs: seq<'A>) = rf.AddRange xs

let writeTo (os: CodedOutputStream) (x: 'A) =
    (x :> IMessage<'A>).WriteTo os

[<EntryPoint>]
let main argv = 
    let allTest =
        new ProtobufUnittest.TestAllTypes (
            SingleBool = true,
            SingleBytes = ByteString.CopyFrom(1uy, 2uy, 3uy, 4uy),
            SingleDouble = 23.5,
            SingleFixed32 = 23u,
            SingleFixed64 = 1234567890123uL,
            SingleFloat = 12.25f,
            SingleForeignEnum = ForeignEnum.ForeignBar,
            SingleForeignMessage = new ForeignMessage ( C = 10 ),
            SingleImportEnum = ImportEnum.ImportBaz,
            SingleImportMessage = new ImportMessage ( D = 20 ),
            SingleInt32 = 100,
            SingleInt64 = 3210987654321L,
            SingleNestedEnum = TestAllTypes_NestedEnum.Foo,
            SingleNestedMessage = new TestAllTypes_NestedMessage ( Bb = 35 ),
            SinglePublicImportMessage = new PublicImportMessage ( E = 54 ),
            SingleSfixed32 = -123,
            SingleSfixed64 = -12345678901234L,
            SingleSint32 = -456,
            SingleSint64 = -12345678901235L,
            SingleString = "test",
            SingleUint32 = UInt32.MaxValue,
            SingleUint64 = UInt64.MaxValue,
            OneofField = TestAllTypes_OneofField.OneofString "Oneof string"
        )
    allTest.RepeatedBool += [false; true]
    allTest.RepeatedBytes += [ ByteString.CopyFrom(1uy, 2uy, 3uy, 4uy); ByteString.CopyFrom(5uy, 6uy); ByteString.CopyFrom(Array.zeroCreate 1000) ]
    allTest.RepeatedDouble += [ -12.25; 23.5 ]
    allTest.RepeatedFixed32 += [ UInt32.MaxValue; 23u ]
    allTest.RepeatedFixed64 += [ UInt64.MaxValue; 1234567890123uL ]
    allTest.RepeatedFloat += [ 100.0f; 12.25f ]
    allTest.RepeatedForeignEnum += [ ForeignEnum.ForeignFoo; ForeignEnum.ForeignBar ]
    allTest.RepeatedForeignMessage += [ new ForeignMessage(); new ForeignMessage( C = 10 ) ]
    allTest.RepeatedImportEnum += [ ImportEnum.ImportBaz; ImportEnum.Unspecified ]
    allTest.RepeatedImportMessage += [ new ImportMessage( D = 20 ); new ImportMessage( D = 25 ) ]
    allTest.RepeatedInt32 += [ 100; 200 ]
    allTest.RepeatedInt64 += [ 3210987654321L; Int64.MaxValue ]
    allTest.RepeatedNestedEnum += [ TestAllTypes_NestedEnum.Foo; TestAllTypes_NestedEnum.Neg ]
    allTest.RepeatedNestedMessage += [ new TestAllTypes_NestedMessage ( Bb = 35 ); new TestAllTypes_NestedMessage ( Bb = 10 ) ]
    allTest.RepeatedPublicImportMessage += [ new PublicImportMessage( E = 54 ); new PublicImportMessage( E = -1 ) ]
    allTest.RepeatedSfixed32 += [ -123; 123 ]
    allTest.RepeatedSfixed64 += [ -12345678901234L; 12345678901234L ]
    allTest.RepeatedSint32 += [ -456; 100 ]
    allTest.RepeatedSint64 += [ -12345678901235L; 123L ]
    allTest.RepeatedString += [ "foo"; "bar" ]
    allTest.RepeatedUint32 += [ UInt32.MaxValue; UInt32.MinValue ]
    allTest.RepeatedUint64 += [ UInt64.MaxValue; uint64 UInt32.MinValue ]

    let ms = new IO.MemoryStream()
    let os = new CodedOutputStream(ms)
    writeTo os allTest
    os.Flush()
    ms.Position <- 0L
    let allTest2 = TestAllTypes.Parser.ParseFrom(ms)

    printfn "\nOriginal:"
    printfn "%O" allTest
    printfn "\nHydrated:"
    printfn "%O" allTest2
    printfn "\nEquals: %b" (allTest.Equals(allTest2))
    0
