
import unittest
import numpy as np
import numpy.testing as npt

class NdArrayBasicsTest(unittest.TestCase):

    def test_shape_size(self):
        a = np.asarray([1,2,3,4,5])
        self.assertEqual(1,a.ndim)
        self.assertEqual((5,),a.shape)
        
        b = np.asarray([[1,2,3],[4,5,6]])
        self.assertEqual(2,b.ndim)
        self.assertEqual((2,3),b.shape)
        self.assertEqual(6,b.size)

        matrixOneRow = np.array([[1,2]])
        self.assertEqual(2,matrixOneRow.ndim)        
        self.assertEqual((1,2),matrixOneRow.shape)

    def test_iterate(self):
        """along the first axis, for a matrix, that is along the row"""
        a = np.arange(6).reshape(2,3)
        self.assertEqual(2,len(a))# len return the number of rows
        # iteration will return each row
        npt.assert_array_equal([[0,1,2],
                                [3,4,5]],[row for row in a])

    def test_index(self):
        a = np.asarray([[1,2,3],
                        [4,5,6]])
        self.assertEqual(4,a[1,0])
        self.assertEqual(2,a[0,1])

        # negative index
        self.assertEqual(6,a[-1,-1])
        self.assertEqual(5,a[-1,1])

        # a whole row or a whole column
        npt.assert_equal([4,5,6],a[1])# 1-th row
        npt.assert_equal([3,6],a[:,2])# 2-th column

    def test_copy(self):
        a = np.asarray([(1,2),
                        (3,4)])
        # copy will return a isolated copy
        cpy = a.copy()

        # changes on copy will not affect original
        cpy[1,1] = -999
        npt.assert_array_equal([[1,    2],
                                [3, -999]],cpy)
        npt.assert_array_equal([(1,2),
                                (3,4)],a)

