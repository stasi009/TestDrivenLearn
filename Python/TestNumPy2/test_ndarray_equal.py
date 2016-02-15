
import unittest
import numpy as np
import numpy.testing as npt

class NdArrayEqualTest(unittest.TestCase):
    
    def test_equalsign_scalar(self):
        """
        compare a ndarray with scalar by using == 
        performs a element-wise equality check
        the result is a boolean array which contains result of each element-wise check
        """
        a = np.asarray([1,2,3])
        npt.assert_equal([True,False,False],a==1)
        npt.assert_equal([False,True,False],a==2)
        npt.assert_equal([False,False,False],a==100)
        
    def test_equalsign_another_array(self):
        a = np.asarray([1,2,3])
        # ------------ same number, element-wise checking
        npt.assert_equal([True,True,True],a==[1,2,3])
        # ------------ different length, always return false
        self.assertFalse(a==[1,2])
        # ------------ single number, broadcasting, and then element-wise checking
        npt.assert_equal([False,False,True],a==[3])
        
    def test_check_content_equal(self):
        # integer matrix
        a = np.asarray([[1,2,3],[4,5,6]])
        at = np.c_[[1,2,3],[4,5,6]]
        self.assertTrue( np.array_equal(at,a.T) )