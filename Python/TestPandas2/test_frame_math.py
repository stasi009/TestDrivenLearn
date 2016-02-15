
import unittest
import numpy as np
import numpy.testing as npt
import pandas as pd
import pandas.util.testing as pdt
import test_testing as tt

class FrameMathTest(unittest.TestCase):

    def test_nonoverlap_nan(self):
        """
        introduces NA values in the indices that don't overlap.
        """
        df1 = pd.DataFrame(np.arange(12.).reshape((3, 4)), columns=list('abcd'))
        df2 = pd.DataFrame(np.arange(20.).reshape((4, 5)), columns=list('abcde'))
        result_nan = df1 + df2

        npt.assert_allclose([[0.,   2.,   4.,   6.,  np.nan],       
                             [9.,  11.,  13.,  15.,  np.nan],       
                             [18.,  20.,  22.,  24.,  np.nan],       
                             [np.nan,  np.nan,  np.nan,  np.nan,  np.nan]],result_nan.values)

    def test_nonoverlap_fillvalue(self):
        """
        add fill_value option to replace NaN when index/column not overlap
        """
        df1 = pd.DataFrame(np.arange(12.).reshape((3, 4)), columns=list('abcd'))
        df2 = pd.DataFrame(np.arange(20.).reshape((4, 5)), columns=list('abcde'))

        # add fill_value to replace NaN
        result_fillvalue = df1.add(df2,fill_value=0)
        npt.assert_allclose([[0.,   2.,   4.,   6.,   4.],       
                             [9.,  11.,  13.,  15.,   9.],       
                             [18.,  20.,  22.,  24.,  14.],       
                             [15.,  16.,  17.,  18.,  19.]],result_fillvalue.values)

    def test_mean(self):
        df = pd.DataFrame(np.arange(1,10).reshape(3,3),
                             columns = ["a","b","c"],
                             index = ["record1","record2","record3"])
        npt.assert_equal([[1, 2, 3],       
                          [4, 5, 6],       
                          [7, 8, 9]],df.values)

        # mean of each column
        mean_each_cols = df.mean()
        pdt.assert_series_equal(pd.Series([4.0,5.0,6.0],index=df.columns),mean_each_cols)

        # mean of each row
        mean_each_rows = df.mean(axis=1)
        pdt.assert_series_equal(pd.Series([2.0,5.0,8.0],index=df.index),mean_each_rows)

    def test_sum(self):
        df = pd.DataFrame(np.arange(1,10).reshape(3,3),
                             columns = ["a","b","c"],
                             index = ["record1","record2","record3"])
        npt.assert_equal([[1, 2, 3],       
                          [4, 5, 6],       
                          [7, 8, 9]],df.values)

        # mean of each column
        sum_each_cols = df.sum()
        pdt.assert_series_equal(pd.Series([12,15,18],index=df.columns),sum_each_cols)

        # mean of each row
        sum_each_rows = df.sum(axis=1)
        pdt.assert_series_equal(pd.Series([6,15,24],index=df.index),sum_each_rows)

    def test_broadcast_along_rows(self):
        """
        By default, arithmetic between DataFrame and Series matches the index of the Series on the DataFrame's columns, broadcasting down the rows
        """
        df = pd.DataFrame(np.arange(1,10).reshape(3,3),
                             columns = ["a","b","c"],
                             index = ["record%d" % i for i in xrange(1,4)])
        mean_each_cols = df.mean()
        mean_removed = df - mean_each_cols
        pdt.assert_frame_equal(pd.DataFrame([[-3., -3., -3.],                                    
                                             [0.,  0.,  0.],                                     
                                             [3.,  3.,  3.]],columns=df.columns,index=df.index),mean_removed)

    def test_broadcast_along_columns(self):
        """
        If you want to instead broadcast over the columns, matching on the rows, 
        you have to use one of the arithmetic methods, and specify axis=0
        """
        df = pd.DataFrame(np.arange(1,10).reshape(3,3),
                             columns = ["a","b","c"],
                             index = ["record%d" % i for i in xrange(1,4)])
        mean_each_row = df.mean(axis=1)
        mean_removed = df.sub(mean_each_row,axis="index")
        pdt.assert_frame_equal(pd.DataFrame([[-1.,  0.,  1.],       
                                             [-1.,  0.,  1.],       
                                             [-1.,  0.,  1.]],columns=df.columns,index=df.index),mean_removed)

    def test_broadcast_sample1(self):
        df = pd.DataFrame(np.arange(1,5).reshape(2,2),index=["r1","r2"],columns=["c1","c2"])

        row_vector = pd.Series([5,6],index=["c1","c2"])
        column_vector = pd.Series([5,6],index=["r1","r2"])

        # ------------ broadcast down the rows
        expect_brdcast_rows = pd.DataFrame([[5,12],
                                            [15,24]],columns=df.columns,index=df.index)
        pdt.assert_frame_equal(expect_brdcast_rows,df.mul(row_vector,axis="columns"))
        pdt.assert_frame_equal(expect_brdcast_rows,df*row_vector)

        # ------------ broadcast along columns
        expect_brdcast_cols = pd.DataFrame([[5,10],
                                            [18,24]],columns=df.columns,index=df.index)
        pdt.assert_frame_equal(expect_brdcast_cols,df.mul(column_vector,axis = "index"))

    

    





