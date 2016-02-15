
import unittest

class ClosureTest(unittest.TestCase):
    def testNewNameCreated(self):
        """
        note: one big difference about the closure in Python and C# is that
        there is possibility to create a totally new name inside the closure, but happen to have same literal name with outside one
        so though it appears to be "same variable", but in fact refer to different object
        
        but in C#, there is no such problem, because in C#, you have to declare some variables with its type and name
        so if you uses a name without declaration, then at compile time, the compiler will transfer that codes into a closure
        it even happen before runtime
        """
        value = 0
        
        def closure(newvalue):
            # create a new name inside the namespace
            # so will not change the outer name
            value = newvalue 
            return value
            
        self.assertEqual(100, closure(100) )
        self.assertEqual(0,value)
        
    def testAccessOuterVariable(self):
        alist = []
        def closure(value):
            alist.append(value)
            
        count = 5
        for index in range(count):
            closure(index)
            
        self.assertSequenceEqual(range(count),alist)

    def testClosureSample(self):
        # this adder1 takes the scope of "closure_sample()"
        adder1 = closure_sample()
        self.assertEqual(1,adder1())
        self.assertEqual(2,adder1())
        
        # this adder2 takes the scope of "closure_sample(6)"
        # a totally different namespace
        adder2 = closure_sample(6)
        self.assertEqual(7,adder2())
        self.assertEqual(8,adder2())
        self.assertEqual(3,adder1())
       
# this sample also demonstrate that 
# python allows definition behind the usage
def closure_sample(start = 0):
    # here must use list, because list itself will not be written
    # only access (read) its element, then will not create new identifier in local scope
    count = [start]
    
    def increment():
        count[0] += 1
        return count[0]
    
    return increment

if __name__ == "__main__":
    unittest.main()