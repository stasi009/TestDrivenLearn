
import unittest
import os

class PathTest(unittest.TestCase):
    
    def testJoin(self):
        filename = os.path.join("c:", "My Documents", "test", "myfile")
        self.assertEqual("c:My Documents\\test\\myfile",filename)
        
    def testSplitExt(self):
        basename,ext = os.path.splitext("helloworld.py")
        self.assertEqual("helloworld",basename)
        self.assertEqual(".py",ext) # pay attention, there is a dot 
        
    def testDirBaseName(self):
        """
        since '\t' is escape character
        so either use 'r' before the string, or use '/' within the string
        otherwise, below tests won't pass
        """
        path = r"data\test.txt"
        self.assertEqual("data",os.path.dirname(path))
        self.assertEqual("test.txt",os.path.basename(path))
        