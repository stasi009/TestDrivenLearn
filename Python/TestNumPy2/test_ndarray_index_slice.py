
import numpy as np
import numpy.testing as npt
import unittest

class NdArrayIndexSliceTest(unittest.TestCase):
    
    def test_slice_view(self):
        """
        !!! this is important, slice return just a view
        !!! not a isolated copy, so change will reflect both way
        !!! between the original array and its sliced view
        """
        original = np.arange(5)
        view = original[::2]
        npt.assert_array_equal([0,2,4],view)
        
        # change on view will reflect on the original array
        view[1] = -9
        npt.assert_array_equal([0,1,-9,3,4],original)
        
        # change on the original array will reflect on view
        original[-1] = -88
        npt.assert_array_equal([0,-9,-88],view)
        
    def test_slice_view2(self):
        original = np.asarray( [[1,2],
                                [3,4]] )
        view = original[:,1]
        npt.assert_array_equal([2,4],view)
        
        # change on view will affect the original
        view[1] = -99
        npt.assert_array_equal([2,-99],view)
        npt.assert_array_equal([[1,2],
                                [3,-99]],original)
        
        # change on original will affect the view
        original[0,-1] *= -1
        npt.assert_array_equal([-2,-99],view)
        
    def test_reverse(self):
        """
        use slice to return a reverse view of the original array
        """
        a = np.array([3,2,1])
        reverse_view = a[::-1]
        npt.assert_array_equal([1,2,3],reverse_view)
        
        reverse_view[0] = -99
        npt.assert_array_equal([3,2,-99],a)
    
    def test_index_singledim_array(self):
        a = np.arange(1,5)
        npt.assert_array_equal ([1,2,3,4],a)
        self.assertEqual(3,a[-2])
        
        with self.assertRaises(IndexError):
            a[-200]
            
    def test_slice_singledim_array(self):
        a = np.arange(1,6)
        npt.assert_array_equal([1,2,3,4,5],a)
        npt.assert_array_equal( [3,4],a[-3:-1]) 
        npt.assert_array_equal( [],a[-1:-3])  
        npt.assert_array_equal( [5,4],a[-1:-3:-1]) 
        
    def test_index_in_matrix(self):
        a = np.eye(4)
        self.assertAlmostEqual(1.0,a[0,0])
        self.assertAlmostEqual(0.0,a[3,0])
        
        with self.assertRaises(IndexError):
            a[0,0,0]
        
        ############## specify multiple indices
        # diagonal elements
        npt.assert_array_equal( np.ones(4),a[range(4),range(4)])
        
    def test_multiple_indices(self):
        a = np.array([[1,2,3],
                      [4,5,6],
                      [7,8,9]])
        rows = [1,0,2]
        cols = [2,1,0]
        npt.assert_array_equal([6,2,7],a[rows,cols])
            
    def test_slice_sample1(self):
        """array[index] is a convenient method to access the rows"""
        a = np.eye(4)
        npt.assert_allclose([1.0,0.0,0.0,0.0], a[0]) 
        npt.assert_allclose([0.0,1.0,0.0,0.0], a[1]) 
        
    def test_slice_rows(self):
        a = np.asarray([[1,2,3,4],
                        [5,6,7,8],
                        [9,10,11,12]])
        
        # two method to access single row
        # ndarray[row] and ndarray[row,:]
        # both methods works
        npt.assert_array_equal( [5,6,7,8],a[1] )
        npt.assert_array_equal( [9,10,11,12],a[2,:] )
        
        # slice multiple rows
        npt.assert_array_equal( [[5,6,7,8],
                                 [9,10,11,12]],a[1:,:])
        
    def test_slice_columns(self):
        a = np.asarray([[1,2,3,4],
                        [5,6,7,8],
                        [9,10,11,12]])
        
        # access single columns
        npt.assert_array_equal( [2,6,10],a[:,1] ) 
        npt.assert_array_equal( [3,7,11],a[:,-2] ) 
        
        # access multiple columns
        npt.assert_array_equal( [[1,2,3],
                                 [5,6,7],
                                 [9,10,11]],a[:,0:3] ) 
        
    def test_slice_continue_block(self):
        a = np.asarray([[1,2,3,4],
                        [5,6,7,8],
                        [9,10,11,12]])
        npt.assert_array_equal([[6,7],
                                [10,11]],a[1:3,1:3])
        
    def test_slice_noncontinue_block(self):
        a = np.asarray([[1,2,3,4],
                        [5,6,7,8],
                        [9,10,11,12]])

        npt.assert_array_equal([[1,3,4],
                                [5,7,8],
                                [9,11,12]],a[:,(0,2,3)])
        npt.assert_array_equal([[1,2,3,4],
                                [9,10,11,12]],a[(0,2),:])
        
        # access individual point
        npt.assert_array_equal( [a[1,3],a[2,0]],a[(1,2),(3,0)] )
        npt.assert_array_equal( [1,8], a[(0,1),(0,3)] )
        
        # to slice a non-continuous block
        with self.assertRaises(IndexError):
            a[(0,2),(0,2,3)] # it thoughs it wants to access individual elements
            
        npt.assert_array_equal( [[1,3,4],
                                 [9,11,12]],a[(0,2),:][:,(0,2,3)] )
        
    def test_slice_by_array(self):
        """use another array as indices to slice the matrix"""
        a = np.asarray([[1,2,3,4],
                        [5,6,7,8],
                        [9,10,11,12]])
        b = np.asarray([0,2])
        
        npt.assert_array_equal( [[1,2,3,4],
                                 [9,10,11,12]],a[b,:])
        npt.assert_array_equal( [[1,3],
                                 [5,7],
                                 [9,11]],a[:,b])
        
        
        
        
        