
import unittest

def safe_int(obj):
    try:
        return int(obj)
    except (ValueError,TypeError):
        return "ErrorCast"
    
def throw_exception(value1,value2,value3):
    """throw tuple as the argument of exception"""
    raise Exception(value1,value2,value3)
    
def not_implemented_method():
    raise NotImplementedError("Test")

class ExceptionTest(unittest.TestCase):
    
    class Fool(object):
        def __init__(self):
            self._finally = False
            
        def run(self,success):
            try:
                if not success: raise ValueError("just for test")
            finally:
                self._finally = True
                
        def __bool__(self): return self._finally
    
    def testSample1(self):
        self.assertEqual(3,safe_int(3.14))
        self.assertEqual("ErrorCast",safe_int("3.14"))
        
    def testErrorMessage(self):
        try:
            not_implemented_method()
        except NotImplementedError as error:
            self.assertEqual("Test",str(error))
            
    def testTupleArgs1(self):
        filename = "noexist.txt"
        try:
            fr = open(filename,"rt") # "rt" stands for "read text" ('t' is default mode)
        except IOError as error:
            self.assertEqual(2,len(error.args))
            self.assertEqual(error.args[0],error.errno)
            self.assertEqual(error.args[1],error.strerror)
            # although IOError will have "filename" attribute, but that attribute will not be included in the tuple
            self.assertEqual(filename,error.filename)
        else:
            self.fail("an exception is expected")
            
    def testTupleArgs2(self):
        """test the usage that use tuple as the argument of exception"""
        try:
            throw_exception(1,"cheka",3.14)
        except Exception as error:
            arg1,arg2,arg3 = error.args # __getitem__ has been overriden, so can retrieve values directly
            self.assertEqual(1,arg1)
            self.assertEqual("cheka",arg2)
            self.assertAlmostEqual(3.14,arg3)
        else:
            self.fail("it should throw out an exception")
            
    def testFinally(self):
        def fool(success):
            f1 = ExceptionTest.Fool()
            self.assertFalse(f1)
            
            try: 
                f1.run(success)
            except ValueError:
                pass
            
            self.assertTrue(f1)
            
        fool(True)
        fool(False)
            
        
if __name__ == "__main__":
    unittest.main()
    