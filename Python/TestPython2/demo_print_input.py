
def demo_print_multiples():
    a = 1
    b = "stasi"
    c = 99.9
    d = True
    # print multiple items on one line, seperated by whitespace
    print(a,b,c,d)

def demo_howto_print_oneline():
    ################### print in multiple lines
    for c in ["a","b","c"]:
        print(c)

    ################### print in single line
    for c in ["a","b","c"]:
        print(c,end="")

# only the specific character used to quote the string needs to be escaped.
# so in a single-quoted string, double-quote is not necessary to escape
def demo_escape():
    print ("So I said, \"You don't know me! You'll never understand me!\"")
    print ('So I said, "You don\'t know me! You\'ll never understand me!"')
    print ("This will result in only three backslashes: \\ \\ \\")
    print ("""The double quotation mark (") is used to indicate direct quotations.""")

def demo_input_string():
    userinput = raw_input("please input: ")
    print("your input is: " + userinput)

def demo_input_numbers():
    num1 = int (input("give me a number: "))
    num2 = int (input("give me another number: "))
    print("total is %d"%(num1+num2))

if __name__ == "__main__":
    # demo_print_multiples()
    # demo_escape()
    # demo_input_string()
    # demo_input_numbers()
    demo_howto_print_oneline()
