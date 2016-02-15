
import numpy as np
import numpy.testing as npt
import unittest

class ConcatenateTest(unittest.TestCase):
    def test_c_r_onedim(self):
        a = np.asarray( [1,2,3] )
        b = np.asarray( [4,5,6] )
        
        con_as_column = np.asarray([[1,4],[2,5],[3,6]])
        npt.assert_array_equal(con_as_column,np.c_[a,b])
        
        con_as_row = np.asarray([1,2,3,4,5,6])
        npt.assert_array_equal(con_as_row,np.r_[a,b])
        
    def test_c_1(self):
        a = np.asarray( [[1,2],[3,4],[5,6]] )
        a = np.c_[a,[7,8,9]]
        expected = np.asarray( [[1,2,7],[3,4,8],[5,6,9]] )
        npt.assert_array_equal(expected, a)
        
        a = np.c_[a,[-1,-2,-3]]
        expected = np.asarray( [[1,2,7,-1],[3,4,8,-2],[5,6,9,-3]] )