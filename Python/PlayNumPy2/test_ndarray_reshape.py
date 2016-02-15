
import unittest
import numpy as np
import numpy.testing as npt

class NdArrayReshapeTest(unittest.TestCase):

    def test_newaxis(self):
        a = np.arange(1,4)
        m = a[:,np.newaxis]
        self.assertEqual((3,1),m.shape)
        npt.assert_equal([[1],       
                          [2],       
                          [3]],m)

    def test_flat(self):
        """
        return `numpy.flatiter` instance, which acts similarly to, but is not a subclass of, Python's built-in iterator object.
        """
        a = np.arange(6).reshape((2,3))

        # use "flat" to iterate each elements
        element_iterator = a.flat
        self.assertEqual(0,next(element_iterator))
        self.assertEqual(1,next(element_iterator))
        self.assertEqual([2,3,4,5],list(element_iterator))

    def test_flatten(self):
        """flatten performs in row fashion"""
        a = np.array([[1,2],
                      [3,4],
                      [5,6]])
        self.assertEqual((3,2),a.shape)
        # default direction is "C", which represents "C language", that is,
        # row-major

        flat_copy = a.flatten()
        npt.assert_array_equal([1,2,3,4,5,6],flat_copy)
        # "F" means "Fortran" style, that is, column-major
        npt.assert_array_equal([1,3,5,2,4,6],a.flatten('F'))

        # change the original won't affect the copy
        a[1,1] = -99
        npt.assert_array_equal([1,2,3,4,5,6],flat_copy)
        npt.assert_array_equal([1,2,3,-99,5,6],a.flatten())# to see the changes, you have to call flatten again

    def test_ravel(self):
        x = np.array([[1, 2, 3], 
                      [4, 5, 6]])
        view = x.ravel()
        npt.assert_equal([1,2,3,4,5,6],view)

        # bi-direction change
        x[1,1] = -999
        npt.assert_equal([1,2,3,4,-999,6],view)

    def test_c_r_demo1(self):
        a = [1,2,3] 
        b = [4,5,6]
        
        npt.assert_array_equal([[1,4],             
                                [2,5],             
                                [3,6]],np.c_[a,b])
        
        npt.assert_array_equal([1,2,3,4,5,6],np.r_[a,b])

    def test_c_r_demo2(self):
        a = np.arange(4).reshape(2,2)
        npt.assert_equal([[0, 1],      
                          [2, 3],       
                          [0, 1],       
                          [2, 3]],np.r_[a,a])
        npt.assert_equal([[0, 1, 0, 1],       
                          [2, 3, 2, 3]],np.c_[a,a])
        

    def test_append_column(self):
        a = np.asarray([[1,2],
                        [3,4],
                        [5,6]])
        a = np.c_[a,[7,8,9]]
        npt.assert_array_equal([[1,2,7],
                                [3,4,8],
                                [5,6,9]], a)
        a = np.c_[a,[-1,-2,-3]]
        npt.assert_array_equal([[1,2,7,-1],
                                [3,4,8,-2],
                                [5,6,9,-3]],a)

    def test_reshape1(self):
        """by default, reshape is row-major, fill the whole row and then fill another row"""
        a = np.arange(6)

        # multiple ways to specify the shape
        # -------------- you can pass in as separate numbers
        m1 = a.reshape(2,3)

        # -------------- you can pass in as tuple
        m2 = a.reshape((2,3))

        # -------------- one dimension can be -1, so its value will be inferred
        m3 = a.reshape(2,-1)

        # -------------- they will get the same result
        expected = [[0,1,2],
                    [3,4,5]]
        npt.assert_array_equal(expected,m1)
        npt.assert_array_equal(expected,m2)
        npt.assert_array_equal(expected,m3)

    def test_reshape_resize(self):
        original = np.asarray([[0,1,2],        
                               [3,4,5]])

        # it isn't transpose
        expected = [[0, 1],            
                    [2, 3],                
                    [4, 5]]

        # --------------- reshape follow row-major fashion
        # --------------- just think, the original will be flattened, and fill
        # the target array in row-first style
        npt.assert_equal(expected,original.reshape((3,2)))
        npt.assert_equal([[0,1,2],     
                          [3,4,5]],original)# return a reshaped one, but the original isn't changed

        # --------------- resize change in place, modify the original directly
        original.resize((3,2))
        npt.assert_equal(expected,original)# return a reshaped one, but the original isn't changed

    def test_reshape_standalone_func(self):
        """reshape, elements are taken and filled in row-wise order"""
        original = np.asarray([[1, 2, 3, 4],                             
                               [5, 6, 7, 8]])
        
        reshaped = np.reshape(original,(4,2))
        # in row-wise order
        npt.assert_equal([[1,2],
                           [3,4],
                           [5,6],
                           [7,8]],reshaped)
        self.assertEqual((2,4),original.shape)        

    def test_reshape_return_view(self):
        """
        !!! It is not always possible to change the shape of an array without copying the data.

        however, most of my usage, array returned by reshape is just view of the original
        so modify on the returned array will directly impact the original one and vice versa
        """
        original = np.array([[1, 2, 3, 4],
                             [5, 6, 7, 8]])
                
        reshaped = original.reshape(4,2)
        # in row-wise order
        npt.assert_equal([[1,2],     
                           [3,4],
                           [5,6], 
                           [7,8]],reshaped)
        
        ################## modify on the returned view, impact the original
        ################## directly
        reshaped[-1,0] = -999
        self.assertEqual(-999,original[1,2])
        
        ################## modify on the original, impact the returned view
        ################## directly
        original[0,2] = -888
        self.assertEqual(-888,reshaped[1,0])
        
        ##################
        npt.assert_equal([[1, 2, -888, 4],                                   
                           [5, 6, -999, 8]],original)
        npt.assert_equal([[1,2],                                    
                           [-888,4],
                           [5,6],
                           [-999,8]],reshaped)

    def test_transpose(self):
        a = np.asarray([[0,1,2],               
                       [3,4,5]])
        at = a.T
        npt.assert_equal([[0, 3],       
                          [1, 4],       
                          [2, 5]],at)

        # changes on one will affect the other
        a[1,1] = -999
        npt.assert_equal([[0, 3],       
                          [1, -999],       
                          [2, 5]],at)

        # transpose only work for array whose ndim > 1.  one-dimensional
        # array's transpose will return itself
        onedim_array = np.asarray([1,2])
        npt.assert_equal(onedim_array,onedim_array.T)


    def test_stack(self):
        a = np.asarray([[1,2],
                        [3,4]])
        npt.assert_array_equal([[1,2],
                                 [3,4],
                                 [1,2],
                                 [3,4],
                                 [1,2],
                                 [3,4]],np.vstack((a,a,a)))
        npt.assert_array_equal([[1,2,1,2],
                                 [3,4,3,4]],np.hstack([a,a]))

    def test_squeeze(self):
        oneelem = np.array([[6]])
        self.assertEqual((1,1),oneelem.shape)
        
        scalar = np.squeeze(oneelem)
        self.assertEqual(6,scalar)
        self.assertEqual(6,int(scalar))
        self.assertTrue(not isinstance(scalar,int))
        
        with self.assertRaises(IndexError):
            # zero-dim, cannot be indexed
            self.assertEqual(6,scalar[0])
        
        self.assertEqual(0,scalar.ndim)
        self.assertEqual((),scalar.shape)

    def test_split_equal_chunk(self):
        """
        if pass in a single number, it means split into equal-sized chunck
        """
        a = np.arange(16).reshape(4,4)

        # ----------- divide into two horizontal chunks
        left,right = np.hsplit(a,2)
        npt.assert_equal([[0,  1],        
                          [4,  5],        
                          [8,  9],        
                          [12, 13]],left)
        npt.assert_equal([[2,  3],        
                          [6,  7],        
                          [10, 11],        
                          [14, 15]],right)

        # ----------- divide into two vertical chunks
        upper,lower = np.vsplit(a,2)
        npt.assert_equal([[0, 1, 2, 3],       
                          [4, 5, 6, 7]],upper)
        npt.assert_equal([[8,  9, 10, 11],       
                          [12, 13, 14, 15]],lower)

    def test_split_by_positions(self):
        a = np.arange(16).reshape(4,4)
        splits = np.hsplit(a,[1,3])
        npt.assert_equal([[0],        
                          [4],        
                          [8],        
                          [12]],splits[0])# first is before column 1
        npt.assert_equal([[1,  2],        
                          [5,  6],        
                          [9, 10],        
                          [13, 14]],splits[1])# second is from 1 to 2
        npt.assert_equal([[3],        
                          [7],        
                          [11],        
                          [15]],splits[2])

        
        
