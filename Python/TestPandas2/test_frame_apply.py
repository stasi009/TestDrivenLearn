
import unittest
import numpy as np
import numpy.testing as npt
import pandas as pd
import pandas.util.testing as pdt

class FrameApplyTest(unittest.TestCase):

    def test_apply_return_scalar(self):

        def span(series): # series can be either row or column
            return series.max() - series.min()

        df = pd.DataFrame(np.arange(1,10).reshape(3,3),
                             columns = ["a","b","c"],
                             index = ["record%d" % i for i in xrange(1,4)])

        # apply on each column
        pdt.assert_series_equal(pd.Series([6,6,6],index=df.columns),   df.apply(span))

        # apply on each row
        pdt.assert_series_equal(pd.Series([2,2,2],index=df.index),   df.apply(span,axis=1))

    def test_apply_return_series(self):
        def span(series):# series can be either row or column
            return pd.Series([series.min(),series.max()],index=["min","max"])

        df = pd.DataFrame(np.arange(1,10).reshape(3,3),
                             columns = ["a","b","c"],
                             index = ["record%d" % i for i in xrange(1,4)])

        # apply on each column
        # !!!  returned series are organized as column
        pdt.assert_frame_equal(pd.DataFrame([[1, 2, 3],       
                                             [7, 8, 9]],columns=["a","b","c"],index=["min","max"]), 
                               df.apply(span))

        # apply on each row
        # !!!  returned series are organized as row
        pdt.assert_frame_equal(pd.DataFrame([[1, 3],       
                                             [4, 6],       
                                             [7, 9]],columns=["min","max"],index=df.index), 
                               df.apply(span,axis=1))

    def test_applymap(self):
        """
        element-wise on each element
        """
        df = pd.DataFrame(np.arange(1,5).reshape(2,2),
                             columns = ["a","b"],
                             index = ["record%d" % i for i in xrange(1,3)])
        newdf = df.applymap(lambda x : x * x)

        pdt.assert_frame_equal(newdf,pd.DataFrame([[1,  4],       
                                                   [9, 16]],columns=df.columns,index=df.index))

        

