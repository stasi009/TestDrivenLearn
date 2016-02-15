
import sys 

def demo_howto_getargs():
    args = sys.argv
    # first argument is always script's name
    assert(args[0] == "demo_commandlineargs.py")
    
    for index,arg in zip(range(len(args)),args):
        print("[%d]: %s"%(index,arg))
        
if __name__ == "__main__":
    demo_howto_getargs()