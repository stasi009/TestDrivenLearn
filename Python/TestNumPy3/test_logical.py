
import numpy as np
import numpy.testing as npt
import unittest

class LogicalTest(unittest.TestCase):
    
    def test_and(self):
        a = np.arange(1,5)
        
        b1 = a > 2
        npt.assert_equal([False,False,True,True],b1)
        
        b2 = a%2 == 0
        npt.assert_equal([False,True,False,True],b2)
        
        composite = np.logical_and(b1,b2)
        npt.assert_equal([False,False,False,True],composite)