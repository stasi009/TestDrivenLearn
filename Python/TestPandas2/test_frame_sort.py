
import unittest
import numpy as np
import numpy.testing as npt
import pandas as pd
import pandas.util.testing as pdt

class FrameSortTest(unittest.TestCase):

    def test_sort_index(self):
        df = pd.DataFrame(np.arange(8).reshape((2, 4)), index=['three', 'one'],columns=['d', 'a', 'b', 'c'])

        # sort row index
        pdt.assert_frame_equal(pd.DataFrame([[4, 5, 6, 7],                                                   
                                             [0, 1, 2, 3]],columns=df.columns,index=["one","three"]),
                               df.sort_index(),check_dtype=False)

        # sort row column names
        pdt.assert_frame_equal(pd.DataFrame([[1, 2, 3, 0],       
                                             [5, 6, 7, 4]],columns=list("abcd"),index=df.index),         
                               df.sort_index(axis=1),check_dtype=False)

        # sort in descending order
        pdt.assert_frame_equal(pd.DataFrame([[0, 3, 2, 1],       
                                             [4, 7, 6, 5]],columns=list("dcba"),index=df.index),         
                               df.sort_index(axis=1,ascending=False),check_dtype=False)

    def test_sort_value_by_index(self):
        df = pd.DataFrame({'b': [4, 7, -3, 2], 'a': [0, 1, 0, 1]},index=[ "r%d" % i for i in xrange(1,5)])

        # by a single column
        pdt.assert_frame_equal(df.sort_index(by="b"),pd.DataFrame([[0, -3],        
                                                                   [1,  2],             
                                                                   [0,  4],           
                                                                   [1,  7]],columns=["a","b"],index=["r3","r4","r1","r2"]))

        # by multiple columns
        # and sort in place
        df.sort_index(by=["a","b"],inplace=True,ascending=False)
        pdt.assert_frame_equal(pd.DataFrame([[1,  7],       
                                             [1,  2],       
                                             [0,  4],       
                                             [0, -3]],columns=df.columns,index=["r2","r4","r1","r3"]), 
                               df)

