
import unittest

class LambdaTest(unittest.TestCase):
    def testDefaultArgs(self):
        self.assertEqual(5,(lambda x,y=2 : x+y)(3))
        
    def testVariableLenArgs(self):
        func = lambda *argtuple : argtuple
        self.assertEqual((2,3),func(2,3))
        
    def testFixedWhenDefine(self):
        x = 10
        y = 5
        func = lambda z=y : x+z
        self.assertEqual(15,func())
        
        # chekanote: the default argument is fixed when function is defined
        # default argument will not be changed later
        y = 8
        self.assertEqual(15,func())
        
    def testClosure(self):
        def make_incrementor(seed): return lambda x: x + seed     
        
        incrementor1 = make_incrementor(1)
        self.assertEqual(2,incrementor1(1))
        
        incrementor2 = make_incrementor(100)
        self.assertEqual(101,incrementor2(1))