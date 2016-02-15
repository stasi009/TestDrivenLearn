
import unittest
from packages1.person import Person

global_value = 101

########################################################################
class FunctionTest(unittest.TestCase):
    """demonstrate how to define and use functions"""
    
    def testDefaultArgs(self):
        def _fool (n1,n2=9): return n1+n2
        
        self.assertEqual(5,_fool(2,3))
        self.assertEqual(10,_fool(1))
        
    def testVariableListArgs(self):
        def _fool(arg1,*args) : 
            assert isinstance(args,tuple),"variable-length arguments are grouped as tuple"
            return (arg1,) + args
        
        self.assertEqual((1,),_fool(1))
        self.assertEqual((1,2,"stasi"),_fool(1,2,"stasi"))
        self.assertEqual(("stasi","cheka","kgb",9),_fool("stasi","cheka","kgb",9))
        
    def testVariableDictArgs(self):
        def _fool(arg1,arg2,**args):
            assert isinstance(args,dict)
            total = arg1+arg2
            if "arg3" in args: return total+args["arg3"]
            else: return total
            
        self.assertEqual(10,_fool(1,3,arg3=6))
        self.assertEqual(4,_fool(1,3))
        self.assertEqual(4,_fool(1,3,notwanted=6))
        
    def testNamedArgs(self):
        def _fool(arg1,start=0,end=10):
            return arg1,start,end
        
        self.assertEqual((9,0,10),_fool(9))
        self.assertEqual((6,"stasi","cheka"),_fool(6,end="cheka",start="stasi"))
        self.assertEqual((8,0,"mss"),_fool(8,end="mss"))
        self.assertEqual((7,"kgb",10),_fool(7,start="kgb")) 
        
    def testNamedArgs2(self):
        """using named argument with function having no default argument"""
        aperson = Person(name="cheka",ssn=1)
        self.assertEqual(1,aperson._ssn)
        self.assertEqual("cheka",aperson._name)
        
        self.assertRaises(TypeError, lambda : Person(ssn = 2) )
        self.assertRaises(TypeError, lambda : Person(ssn = 2,nokey = "no such key") )        
        
    def testPassMutableArgs(self):
        """
        this testcase checks that parameter passing is always "address passing" in python
        """
        def simple_function(alist):
            alist = [] # redirect to a totally new object
            alist.append(1)
            return alist
        
        empty_list = []
        returned_list = simple_function(empty_list)
        
        self.assertEqual([],empty_list)
        self.assertEqual([1],returned_list)
        
    def testPassImmutableArgs(self):
        def _fool(num):
            # No outside effect; lets the local iint to point to a new int object,
            # losing the reference to the int object passed as an argument            
            num += 1
            return num
            
        num = 5
        newnum = _fool(num)
        
        self.assertEqual(5,num)
        self.assertEqual(6,newnum)
        
    def testUnboundLocalError(self):
        """
        assignment within the function shadow the same-name-variable in the parent scope
        """
        counter = 0
        def fool():
            counter += 1
            return counter
        
        with self.assertRaises(UnboundLocalError):
            fool()
        
        ################ the correct way should be like below
        counters = [0]
        def good():
            counters[0] += 1
            return counters[0]
        self.assertEqual(1,good())
        self.assertEqual(1,counters[0])
        
        
    def testNestedScope(self):
        def bar():
            x = 5 # x is now in scope
            return x + y # y is defined in the enclosing scope later
        y = 10
        self.assertEqual( 15,bar() )# now that y is defined, bar's scope includes y   
        
    def testDelegate(self):
        """test the "Delegate" feature in Python, that passes function as argument"""
        # ====================== define isloated function
        def double_number(value):
            return value * 2
        
        # ====================== define instance methods
        class Multiplier(object):
            def __init__(self,times):
                self._times = times
                
            def setTimes(self,times):
                self._times = times
            
            def multiple(self,value):
                return value * self._times
            
        # ====================== define delegate consumer
        def transform_number(value,funclet):
            return funclet(value)
        
        # ====================== test
        self.assertEqual(10,transform_number(5,double_number))
        
        mulobj = Multiplier(5)
        self.assertEqual(90,transform_number(18,mulobj.multiple))
        
        mulobj.setTimes(101)
        self.assertEqual(505,transform_number(5,mulobj.multiple))   
    
    def testGlobalKeyword(self):
        def _shadowGlobal(newvalue):
            """
            by creating a local identifier with same name as global one
            glboal identifier is shadowed in the function's scope
            """
            global_value = newvalue # this 'global_value' is a local one
            return global_value 
            
        def _useGlobalKeyword(newvalue):
            """by using "global" keyword, same-name identifier will not shadow global ones"""
            global global_value
            global_value = newvalue        

        # ------------- shadow
        value_inside_func = _shadowGlobal("cheka")
        self.assertEqual("cheka",value_inside_func)
        self.assertEqual(101,global_value)
        
        # ------------- global keyword
        _useGlobalKeyword("stasi")
        self.assertEqual("stasi",global_value)    
        
        
if __name__ == "__main__":
    unittest.main()
    
    
        
        
    
    