
def demo_readline() :
    with open("datas/ZenOfPython.txt","rt") as file:# 'r'-->read, 't'-->text mode
        counter = 0
        txt = file.readline()
        while txt != "":
            counter += 1
            # what we read includes the "newline breaker"
            # if we print them directly, it will have the same effect as printing "\n\n"
            # use rstrip() to remove the "newline breaker" at the end of each line
            print("[%d] %s"%(counter,txt.rstrip()))
            txt = file.readline()

def demo_file_as_iterable():
    file_object = open("myfile", 'r')
    count = 0
    for line in file_object:
        count = count + 1
    print(count)
    file_object.close()
            
def demo_readlines() :
    with open("datas/ZenOfPython.txt","rt") as file:
        lines = file.readlines()
        for index,line in enumerate(lines):
            print("<%d>:\t%s"%(index+1,line.rstrip()))
            
def demo_write():
    with open("datas/testwrite.txt","wt") as outfile:
        lines = ["hello","python","from","sifang"]
        
        # writing them directly, won't automatically add newline breaker
        for line in lines:
            outfile.write(line)
            
        # we have to add "newline breaker" manually
        # any \n characters are mapped back to the platform-specific line endings 
        # that is, '\r\n'on Windows or '\r'on Macintosh platforms
        outfile.write("\n\n\n")
        for line in lines:
            outfile.write(line+"\n")
            
def demo_writelins():
    """
    writelines, something misnomer
    it is just a convenient methods which will write a list of strings into file
    it won't automatically write "newline breaker" either
    """
    with open("datas/testwrite.txt","wt") as outfile:
            lines = ["I","love","tingting"]
            
            # won't add newline breaker for us, all strings are concatenated together
            outfile.writelines(lines)
                
            lines_with_breaker = (s+"\n" for s in lines)
            outfile.writelines(lines_with_breaker)
            
if __name__ == "__main__":
    # demo_readline()
    # demo_readlines()
    # demo_write()
    demo_writelins()