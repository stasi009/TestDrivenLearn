
import unittest

class DemoTest(unittest.TestCase):
    
    # setUp will be invoked before each "testXXX" method
    def setUp(self):
        self.counter = 0
        print("****************** SET UP    ******************")
        
    # tearDown will be invoked after each "testXXX" method
    def tearDown(self):
        print("------------------ TEAR DOWN ------------------")
        
    # before this method, setUp will "reset" the variable
    def test1(self):
        self.counter += 1
        self.assertEqual(1,self.counter)
        
    def test2(self):
        self.counter += 2
        self.assertEqual(2,self.counter)    
    
if __name__ == "__main__":
    unittest.main()