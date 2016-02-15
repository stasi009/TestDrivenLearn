
module TestPrint
// so called "top-level module", no indent is needed
printfn "############# welcome to module <TestPrint> #############"

open TestFSharps.Records

let private sample_print() =
        let city = "Pullman"
        let temperature = 25.0
        printfn "The temperature of '%s' is: %0.3f" city temperature

let private sample_printargs (id:int) (name:string) (score:double) =
    printfn "id=%d name='%s' score=%3.3f" id name score

let private sample_print_records()=
    let movie = {
        Title="story";
        Year=2008;
        Actors=[|"Tom";"Mary"|];
        Score = 5}

    // "%A" uses so-called "generic structure formatting"
    printfn "****** using Format-A (Generic Structure Formatting): \n%A" movie

    // chekanote: "%O" uses "object.ToString()", because this record doesn't override "ToString()"
    // so here it will only print type's name by default
    printfn "****** using Format-O (Object.ToString): \n%O" movie

let test_main()=
    sample_print_records()

