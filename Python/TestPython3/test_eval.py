
import unittest

########################################################################
class EvalTest(unittest.TestCase):        
    def testReprEval(self):
        """
        repr and eval is a pair of reverse function
        
        repr : object --> string
        eval : string --> object
        """
        ori_value = 100
        repr_string = repr(ori_value)
        cpy_value = eval(repr_string)
        self.assertEqual(ori_value,cpy_value)
        
    def testDemoEval(self):
        self.assertEqual(100,eval("80 + 20"))
        
if __name__ == "__main__":
    unittest.main()
