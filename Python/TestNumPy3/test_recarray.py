
import numpy as np
import numpy.testing as npt
import unittest
from util4test import *

class RecArrayTest(unittest.TestCase):
    def test_dim_difference(self):
        fields_array = np.array([(1.1, 2), (3.0, 4)], dtype=[('x', float), ('y', int)])
        matrix = np.array([(1.1, 2), (3.0, 4)])
        
        self.assertEqual((2,),fields_array.shape)
        self.assertEqual((2,2),matrix.shape)
        
        npt.assert_almost_equal(fields_array["x"],matrix[:,0])
        npt.assert_equal(fields_array["y"],matrix[:,1])
        
    
    def test_sample1(self):
        x = np.array([(1.0, 2), (3.0, 4)], dtype=[("score", float), ("id", int)])
        assert_ndarray_almost_equal (self,[1.0,3.0],x["score"])
        assert_ndarray_equal( self,[2,4],x["id"])
        
    def test_sample2(self):
        # we cannot use below line of code
        # because we have to specify the maximum length of the string
        # and below method have no way to specify the length
        # x = np.empty((2,),dtype=[("name",np.str),("id",int),("score",float)])
        x = np.empty((2,),dtype="a8,i4,f8")
        x.dtype.names = ("name","id","score")
        
        names = ["stasi","cheka"]
        ids = [8,9]
        scores = [99.6,88.9]
        
        # only zip is not enough, zip return a "zip object" which will be lazy evaluated
        # we have to use list or tuple to actively evaluate it and then pass into ndarray
        x[:] = list(zip(names,ids,scores))
        self.assertEqual(1,x.ndim) # it is not a matrix
        self.assertEqual((2,),x.shape)
        
        assert_ndarray_equal( self,ids,x["id"])
        
        # before this line, x is still a normal ndarray(with only one dimension)
        x = x.view(np.recarray) # now get a view of "recarray"
        # !!! x.name <> np.array(names), not because of different contents, but because of encoding issue
        # assert_ndarray_equal( self,names,x.name)
        # equal with bytes literal
        assert_ndarray_equal( self,[b"stasi",b"cheka"],x.name)
        assert_ndarray_almost_equal( self,scores,x.score)
        
        