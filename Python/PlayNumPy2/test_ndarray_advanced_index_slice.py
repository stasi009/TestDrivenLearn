
import unittest
import numpy as np
import numpy.testing as npt

class NdArrayAdvancedIndexSliceTest(unittest.TestCase):
    """
    not all slice on ndarray will return view
    advanced slice will return copy

    advanced slicing when: 
    • a non-tuple sequence object, 
	• an ndarray (of data type integer or bool), or 
	• a tuple with at least one sequence object or ndarray (of data type integer or bool). 

    the general principle is:
    ** if the index can slice out a continuous region, then it is basic slicing, return view
    ** otherwise, it cannot slice out a continous region, it is advanced slicing, return copy
    """

    def test_ix_(self):
        a = np.asarray( [[1,2,3],
                         [4,5,6],
                         [7,8,9]] )

        rows = [0,2]
        cols = [2,1]

        # fancy index, return 1-d array
        npt.assert_equal( [3, 8], a[rows,cols] )

        # sub-matrix
        npt.assert_equal([[3, 2],       
                          [9, 8]],a[rows,:][:,cols])

        # sub-matrix with ix_
        ixgrid = np.ix_(rows,cols)
        npt.assert_equal([[3, 2],       
                          [9, 8]],a[ixgrid])


    def test_diff_tuple_list(self):
        a = np.asarray( [[1,2,3],
                         [4,5,6],
                         [7,8,9]] )

        # --------- use tuple, select a single element, interpret as row and column
        self.assertEqual(7, a[(2,0)])

        # --------- use list or ndarray, return multiple rows
        npt.assert_equal ([[7,8,9],                          
                           [1,2,3]], a[[2,0]])

    def test_advanced_slice_return_copy(self):
        
        original = np.arange(12).reshape(3,4)
        npt.assert_array_equal([[0,  1,  2,  3],
                                [4,  5,  6,  7],
                                [8,  9, 10, 11]],original)

        # a discrete range will trigger advaned slicing
        # and advanced slicing always return copy
        sliced = original[:,(1,2)]
        npt.assert_array_equal([[1,  2],
                                [5,  6],
                                [9, 10]],sliced)

        # make changes on the copy
        sliced[:] = -99

        # changes NOT reflect on the original
        npt.assert_array_equal([[0,  1,  2,  3],
                                [4,  5,  6,  7],
                                [8,  9, 10, 11]],original)

    def test_bool_slice_return_copy(self):
        """
        slicing by bool index is always 'advanced slicing'
        so it will always return copy
        """
        a = np.arange(6)
        cpy = a[a % 2 == 0]
        npt.assert_array_equal([0,2,4],cpy)

        # changes on copy will not affect original
        cpy[0] = -999
        npt.assert_array_equal([-999,2,4],cpy)
        npt.assert_array_equal([0,1,2,3,4,5],a)

    def test_multidim_bool_index(self):
        a = np.arange(12).reshape(4,-1)
        npt.assert_equal([[0,  1,  2],       
                          [3,  4,  5],       
                          [6,  7,  8],       
                          [9, 10, 11]],a)

        # !!! pay attention that, the boolean indices must be ndarray, cannot be array-like
        # !!! otherwise, if you pass boolean array, other than boolean ndarray, those boolean in array
        # !!! will be considered as integer 0 or 1
        rows_wanted = np.asarray([False,True,False,True])
        cols_wanted = np.asarray([True,True,False])

        npt.assert_array_equal([[0,  1],
                                [3,  4],
                                [6,  7],
                                [9, 10]],a[:,cols_wanted])

        # sub-matrix
        npt.assert_equal( [[ 3,  4],       
                           [ 9, 10]],a[rows_wanted,:][:,cols_wanted] )

        # **************** test copy feature
        sliced_copy = a[rows_wanted,:] 
        npt.assert_array_equal([[3,  4,  5],                                 
                                [9, 10, 11]],sliced_copy)
        sliced_copy *= -1
        npt.assert_array_equal([[-3,  -4,  -5],
                                 [-9, -10, -11]],sliced_copy)
        npt.assert_equal([[0,  1,  2],       
                          [3,  4,  5],       
                          [6,  7,  8],       
                          [9, 10, 11]],a)# changes on copy won't affect the original

        # todo: broadcast ????????
        npt.assert_array_equal([3, 10],a[rows_wanted,cols_wanted])

    def test_slice_by_array(self):
        """
        use ndarray to slice, triggers advanced slicing, which returns a copy
        """
        a = np.asarray([[1,2,3,4],
                        [5,6,7,8],
                        [9,10,11,12]])
        # no matter the index is array or ndarray, they do the exact same thing
        b = [-1,0]
        c = np.asarray(b)
        
        npt.assert_array_equal([[9,10,11,12],
                                 [1,2,3,4]],a[c,:])
        npt.assert_array_equal([[4,  1],       
                                 [8,  5],       
                                 [12,  9]],a[:,b])
        # slice out individual elements
        npt.assert_equal([12,1],a[b,b])

        # slice out a block
        npt.assert_equal([[12,  9],       
                           [4,  1]],a[c,:][:,b])

        # ------------------ test copy features
        sliced_copy = a[b,:]
        npt.assert_equal([[9,10,11,12],                                 
                          [1,2,3,4]],sliced_copy)

        sliced_copy[:] = -1
        npt.assert_equal([[-1,-1,-1,-1],
                          [-1,-1,-1,-1]],sliced_copy)
        # original isn't affected
        npt.assert_equal([[1,2,3,4],
                          [5,6,7,8],
                          [9,10,11,12]],a)

    def test_slice_rows(self):
        a = np.asarray([[1,2,3,4],
                        [5,6,7,8],
                        [9,10,11,12]])
        
        sliced_copy = a[(-1,0),:]
        npt.assert_array_equal([[9,10,11,12],                      
                                [1,2,3,4]],sliced_copy)

        # test copy feature
        sliced_copy[:] = -1
        npt.assert_array_equal([[1,2,3,4],                        
                                [5,6,7,8],                        
                                [9,10,11,12]],a)

    def test_slice_columns(self):
        a = np.asarray([[1,2,3,4],
                        [5,6,7,8],
                        [9,10,11,12]])
        
        sliced_copy = a[:,(-1,1)]
        npt.assert_array_equal([[4,  2],        
                                [8,  6],         
                                [12, 10]],sliced_copy)

        sliced_copy *= -1
        npt.assert_array_equal([[-4,  -2],        
                                [-8,  -6],         
                                [-12, -10]],sliced_copy)

        # original isn't affected
        npt.assert_array_equal([[1,2,3,4],                        
                                [5,6,7,8],                        
                                [9,10,11,12]],a)

    def test_discrete_index_return_copy(self):
        a = np.asarray([[1,2,3,4],
                        [5,6,7,8],
                        [9,10,11,12]])

        npt.assert_array_equal([[4,1,3,4],
                                [8,5,7,8],
                                [12,9,11,12]],a[:,(-1,0,2,3)])
        
        # access individual point
        self.assertEqual(9,a[2,0])
        # if the indices are two tuples, then first tuple are all row numbers
        # and second tuple are all column numbers
        npt.assert_array_equal([a[1,3],a[2,0]],a[(1,2),(3,0)])
        npt.assert_array_equal([1,8], a[(0,1),(0,3)])
        
        # to slice a non-continuous block
        with self.assertRaises(IndexError):
            a[(0,2),(0,2,3)] # NumPy thoughs it wants to access individual elements
            
        npt.assert_array_equal([[4,1,3],             
                                [12,9,11]],a[(0,2),:][:,(-1,0,2)])

        # ********************** test copy feature
        sliced_copy = a[(-1,0),:]
        npt.assert_array_equal([[9,10,11,12],
                                [1,2,3,4]],sliced_copy)
        sliced_copy[:] = 0
        npt.assert_array_equal([[0,0,0,0],
                                [0,0,0,0]],sliced_copy)
        npt.assert_array_equal([[1,2,3,4],
                                [5,6,7,8],
                                [9,10,11,12]],a)
