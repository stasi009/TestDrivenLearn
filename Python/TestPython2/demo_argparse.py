
import argparse
import sys 

def demo_howto_getargs():
    args = sys.argv
    # first argument is always script's name
    assert(args[0] == "demo_commandlineargs.py")
    
    for index,arg in zip(range(len(args)),args):
        print("[%d]: %s"%(index,arg))

def simple_sample1():
    parser = argparse.ArgumentParser()
    parser.add_argument("echo", help="echo the string you input")
    
    args = parser.parse_args()
    print("echo: '%s'"%(args.echo))
    
def sample_multiple_pos_arguments():
    parser = argparse.ArgumentParser()
    parser.add_argument("n1",help="first number",type=int)
    parser.add_argument("n2",help="second number",type=int)
    
    args = parser.parse_args()
    print("%d + %d = %d"%(args.n1,args.n2,args.n1+args.n2))
    
def sample_specify_argument_type():
    parser = argparse.ArgumentParser()
    parser.add_argument("square",help="display square of a given number",
                        type=int)
    
    args = parser.parse_args()
    print("%d * %d = %d"%(args.square,args.square,args.square**2))
    
def optional_argument_sample1():
    parser = argparse.ArgumentParser()
    parser.add_argument("--verbose",help="verbose or not",type=int)
    
    args = parser.parse_args()
    
    if args.verbose is None:
        print("verbose not specified")
    elif int(args.verbose):
        print("verbose is on")
    else:
        print("verbose is off")
        
def flag_argument_sample():
    parser = argparse.ArgumentParser()
    parser.add_argument("-v","--verbose",help="verbose flag",action="store_true")
    args = parser.parse_args()
    
    assert args.verbose is not None,"if set, True, otherwise, False by default, never None"    
    if args.verbose:
        print("verbose is on")
    else:
        print("verbose is off")
        
def mix_pos_opt_args_choice():
    parser = argparse.ArgumentParser()
    
    # ------------------- add positional arguments
    parser.add_argument("n1",help="first number",type=float)
    parser.add_argument("n2",help="second number",type=float)
    
    # ------------------- add optional arguments
    # and this argument has constraints on its acceptable inputs
    parser.add_argument("-v","--verbosity",help="verbose level",type=int,choices=[1,2,3])
    
    # ------------------- handle inputs
    args = parser.parse_args()
    answer = args.n1 ** 2 + args.n2 ** 2
    
    if args.verbosity is None:
        raise Exception("verbosity is required")
    
    if args.verbosity == 1:
        print("%3.2f**2 + %3.2f**2 = %3.2f"%(args.n1,args.n2, answer))
    elif args.verbosity == 2:
        print("%3.2f^2 + %3.2f = %3.2f"%(args.n1, args.n2, answer))
    else:
        assert args.verbosity == 3
        print(answer)
        
def default_value():
    parser = argparse.ArgumentParser(description="demonstration")
    
    parser.add_argument("n",type=float)
    parser.add_argument("-p","--power",type=float,default=2)
    
    args = parser.parse_args()
    print("%3.2f ** %3.2f = %3.2f"%(args.n,args.power,args.n**args.power))
    
if __name__ == "__main__":
    # sample_specify_argument_type()
    # optional_argument_sample1()
    # sample_multiple_pos_arguments()
    # flag_argument_sample()
    # mix_pos_opt_args_choice()
    default_value()