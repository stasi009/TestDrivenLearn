
import packages1.module1 as module1 # import and rename module
# import packages1.mytime.MyTime as MyTime # !!! import can only import module, cannot be used to import class
from packages1.mytime import MyTime

def demo_import():
    module1.sayHello("tingting")
    print("my wife is: %s"%module1.Wife)
    
    t = MyTime(11,20)
    print("current time: %s"%str(t))
    
    
if __name__ == "__main__":
    demo_import()