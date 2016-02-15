
import unittest
import numpy as np
import numpy.testing as npt

class NdArrayBasicIndexSliceTest(unittest.TestCase):
    """
    the general principle is: if it can slice out a continuous region, it is basic slicing, and return view
    Basic slicing occurs when obj is 
	*. a slice object (constructed by start:stop:step notation inside of brackets), 
	*. an integer, or 
    *. a tuple of slice objects and integers.
    and basic slicing return view
    """
    def test_basic_slice_return_view(self):
        original = np.arange(12).reshape(3,4)
        npt.assert_array_equal([[0,  1,  2,  3],
                                [4,  5,  6,  7],
                                [8,  9, 10, 11]],original)

        # a continous range will trigger basic slice, which will return a view
        sliced = original[:,1:3]
        npt.assert_array_equal([[1,  2],
                                [5,  6],
                                [9, 10]],sliced)

        # make changes on the view
        sliced[:] = -99

        # changes reflect on the original
        npt.assert_array_equal([[0, -99, -99,   3],
                                [4, -99, -99,   7],
                                [8, -99, -99,  11]],original)

    def test_basic_slice_view1(self):
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

    def test_basic_slice_view2(self):
        original = np.asarray([[1,2],
                                [3,4]])
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
        """        use slice to return a reverse view of the original array        """
        a = np.asarray([3,2,1])
        reverse_view = a[::-1]
        npt.assert_array_equal([1,2,3],reverse_view)
        
        reverse_view[0] = -99
        npt.assert_array_equal([3,2,-99],a)

    def test_singledim_array(self):
        a = np.arange(1,6)

        # ------------------------ index
        npt.assert_array_equal([1,2,3,4,5],a)
        self.assertEqual(4,a[-2])
        
        with self.assertRaises(IndexError):
            a[-200]
            
        # ------------------------ slice view
        npt.assert_array_equal([3,4],a[-3:-1]) 
        npt.assert_array_equal([],a[-1:-3])  

        # ------------------------ test view feature
        sliced_view = a[-1:-3:-1]
        npt.assert_array_equal([5,4],sliced_view)
        sliced_view *= -1
        npt.assert_array_equal([1,2,3,-4,-5],a)

    def test_index_single_element_in_matrix(self):
        a = np.asarray([[1,2,3],
                        [4,5,6]])
        self.assertEqual(4,a[1,0])
        self.assertEqual(2,a[(0,1)])# pass in a tuple or two separate numbers, they are the same

        # negative index
        self.assertEqual(6,a[-1,-1])
        self.assertEqual(5,a[(-1,1)])

        # !!!  multiple indices
        # !!!  pay attention that, when pass in a list/tuple of rows, and a
        # list/tuple of columns
        # !!!  it will fetch the elements in each (row,column) pair, and return
        # the values on those positions into a list
        # !!!!!!  it WON'T return a sub-matrix composed by rows and columns
        i1,j1 = 1,1
        i2,j2 = 0,-1
        npt.assert_array_equal([a[i1,j1],a[i2,j2]],a[(i1,i2),(j1,j2)])
        npt.assert_array_equal([5,3],a[(i1,i2),(j1,j2)])

    def test_multiple_indices(self):
        # --------------- indices have same length
        a = np.array([[1,2,3],
                      [4,5,6],
                      [7,8,9]])
        rows = [1,0,2]
        cols = [-2,1,0]
        npt.assert_array_equal([5,2,7],a[rows,cols])

        # --------------- different length, so shorter indices need to broadcast
        # here in below example, the second index is broadcast to [1,1]
        npt.assert_array_equal([8,5],a[[2,1],1])

    def test_slice_rows(self):
        a = np.asarray([[1,2,3,4],
                        [5,6,7,8],
                        [9,10,11,12]])
        
        # two method to access single row
        # ndarray[row] and ndarray[row,:] both methods works
        npt.assert_array_equal([5,6,7,8],a[1])
        npt.assert_array_equal([9,10,11,12],a[2,:])
        
        # slice multiple rows
        npt.assert_array_equal([[5,6,7,8],                                 
                                [9,10,11,12]],a[1:,:])

        # ***************** test view feature
        sliced_view = a[:-1]
        npt.assert_array_equal([[1,2,3,4],                     
                                [5,6,7,8]],sliced_view)

        sliced_view[:] = 0
        npt.assert_array_equal([[0,0,0,0],                        
                                [0,0,0,0],                        
                                [9,10,11,12]],a)

    def test_slice_columns(self):
        a = np.asarray([[1,2,3,4],
                        [5,6,7,8],
                        [9,10,11,12]])
        
        # access single columns
        acolumn = a[:,1]
        self.assertEqual(1,acolumn.ndim) # still return a one-dim row vector, not a column vector
        npt.assert_array_equal([2,6,10],acolumn)
        
        # access multiple columns
        npt.assert_array_equal([[1,2,3],
                                [5,6,7],
                                [9,10,11]],a[:,0:3]) 

        # ***************** test view feature
        sliced_view = a[:,-2]
        npt.assert_equal([3,7,11],sliced_view)

        sliced_view *= -1
        npt.assert_equal([[1,2,-3,4],                        
                          [5,6,-7,8],                        
                          [9,10,-11,12]],a)

    def test_difference_slice_array(self):
        """
        use slice to slice, or use array to slice are different
        """
        a = np.asarray([[1,2,3,4],
                        [5,6,7,8],
                        [9,10,11,12]])

        # return a sub-matrix
        npt.assert_array_equal([[6,7],
                                [10,11]],a[1:3,1:3])

        # although 1:3, is equivalent to [1,2], but pass in [1,2] makes it a advanced indexing
        # and return totally different result
        b = [1,2]
        npt.assert_array_equal([6,11],a[b,b])


    def test_slicer_return_view(self):
        a = np.asarray([[1,2,3,4],
                        [5,6,7,8],
                        [9,10,11,12]])

        sliced_view = a[1:3,1:3]
        npt.assert_array_equal([[6,7],
                                [10,11]],sliced_view)

        sliced_view *= -1
        npt.assert_array_equal([[1,2,3,4],                        
                                [5,-6,-7,8],                        
                                [9,-10,-11,12]],a)

    

    

     