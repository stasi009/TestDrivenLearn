
import unittest
import numpy as np
import numpy.testing as npt
import pandas as pd
import pandas.util.testing as pdt
import test_testing as tt

class FrameIndexSliceTest(unittest.TestCase):
    """
    it seems a rule:
    *** return Series, that is a view
    *** return DataFrame, that is a copy
    """

    def test_in(self):
        df = pd.DataFrame(np.arange(1,10).reshape(3,3),
                             columns = ["a","b","c"],
                             index = ["record1","record2","record3"])

        self.assertTrue("a" in df.columns)
        self.assertTrue("record1" in df.index)

    def test_single_column_view(self):
        frame = pd.DataFrame(np.arange(1,10).reshape(3,3),
                             columns = ["a","b","c"],
                             index = ["record1","record2","record3"])
        col_view = frame.c# frame["c"] also works
        self.assertIsInstance(col_view,pd.Series)
        pdt.assert_series_equal(pd.Series([3,6,9],frame.index),col_view,check_dtype=False,check_names=False)

        # change on the view will affect the original
        col_view[["record1","record3"]] *= -1

        expected = [[1,  2, -3],       
                    [4,  5,  6],       
                    [7,  8, -9]]
        npt.assert_equal(expected,frame.values)

    def test_single_column_copy(self):
        frame = pd.DataFrame(np.arange(1,10).reshape(3,3),
                             columns = ["a","b","c"],
                             index = ["record1","record2","record3"])
        col_copy = frame[["c"]]
        self.assertIsInstance(col_copy,pd.DataFrame)
        npt.assert_equal([[3],
                          [6],
                          [9]],col_copy.values)

        # change on the view will affect the original
        col_copy *= -1
        npt.assert_equal([[-3],
                          [-6],
                          [-9]],col_copy.values)

        # the original isn't changed
        expected = [[1,  2, 3],       
                    [4,  5, 6],       
                    [7,  8, 9]]
        npt.assert_equal(expected,frame.values)

    def test_multi_column(self):
        df = pd.DataFrame(np.arange(1,10).reshape(3,3),
                             columns = ["a","b","c"],
                             index = ["record1","record2","record3"])
        npt.assert_equal([[3, 1],       
                          [6, 4],       
                          [9, 7]],df[["c","a"]].values)

    def test_select_rows_by_slice(self):
        frame = pd.DataFrame(np.arange(1,10).reshape(3,3),
                             columns = ["a","b","c"],
                             index = ["record1","record2","record3"])

        # --------------- single row
        onerow_frame = frame[1:2] # 1-th row
        self.assertIsInstance(onerow_frame,pd.DataFrame)
        npt.assert_equal([[4, 5, 6]],onerow_frame.values)

        # --------------- multiple rows
        sliced_copy = frame[:2]
        npt.assert_array_equal([[1, 2, 3],        
                                [4, 5, 6]],sliced_copy.values)

        sliced_copy *= -1
        npt.assert_array_equal([[-1, -2, -3],        
                                [-4, -5, -6]],sliced_copy.values)

        # the original isn't affected
        npt.assert_array_equal(np.arange(1,10).reshape(3,3),frame.values)

    def test_slice_by_bool_frame(self):
        df = pd.DataFrame(np.arange(1,10).reshape(3,3),
                             columns = ["a","b","c"],
                             index = ["record1","record2","record3"])

        boolframe = pd.DataFrame([[False,  True, False],       
                                  [True, False,  True],       
                                  [False,  True, False]],index = df.index,columns=df.columns)
        pdt.assert_frame_equal(boolframe,df % 2 == 0)

        # selection is also a full dataframe, values not selected will be
        # filled as NaN
        pdt.assert_frame_equal(df[boolframe],pd.DataFrame([[np.nan,   2.,  np.nan],       
                                                           [4.,  np.nan,   6.],       
                                                           [np.nan,   8.,  np.nan]],index=df.index,columns=df.columns))

        # selection can be filled in
        # only those selected will be replaced by the elements the same
        # position in the right-hand-side dataframe
        df[boolframe] = -df 
        pdt.assert_frame_equal(df,pd.DataFrame([[1, -2,  3],       
                                                [-4,  5, -6],       
                                                [7, -8,  9]],index=df.index,columns=df.columns),check_dtype=False)

    def test_select_rows_by_bool_index(self):
        df = pd.DataFrame(np.arange(1,10).reshape(3,3),
                             columns = ["a","b","c"],
                             index = ["record1","record2","record3"])

        boolindex = df.b % 2 == 0
        pdt.assert_series_equal(pd.Series([True,False,True],index=df.index),boolindex,check_names=False)

        selected = df[boolindex]
        npt.assert_array_equal([[1, 2, 3],                                 
                                [7, 8, 9]],selected.values)

    def test_loc1(self):
        """
        label-based indexing and slicing, the label can either be
        * index: row names, or
        * column: column names
        """
        frame = pd.DataFrame(np.arange(1,10).reshape(3,3),
                             columns = ["a","b","c"],
                             index = ["record1","record2","record3"])
        npt.assert_equal([[1, 2, 3],       
                          [4, 5, 6],       
                          [7, 8, 9]],frame.values)

        # ------------------- rows
        npt.assert_equal([[7, 8, 9],       
                          [1, 2, 3]], frame.loc[["record3","record1"]].values)

        # ------------------- columns
        npt.assert_equal([[3, 2],       
                          [6, 5],       
                          [9, 8]], frame.loc[:,["c","b"]].values)

        # ------------------- sub-frame
        sliced_copy = frame.loc[["record1","record2"],
                                ["a","b"]]
        npt.assert_equal([[1, 2],       
                          [4, 5]], sliced_copy.values)
        
        sliced_copy *= -1
        npt.assert_equal([[-1, -2],       
                          [-4, -5]], sliced_copy.values)

        # !!!  won't affect the original
        npt.assert_equal([[1, 2, 3],       
                          [4, 5, 6],       
                          [7, 8, 9]],frame.values)

    def test_loc_bool_index(self):
        df = pd.DataFrame(np.arange(1,10).reshape(3,3),
                             columns = ["a","b","c"],
                             index = ["record1","record2","record3"])
        npt.assert_equal([[1, 2, 3],       
                          [4, 5, 6],       
                          [7, 8, 9]],df.values)

        sliced = df.loc[df.b % 2 == 0,["a","c"]]
        npt.assert_array_equal([[1,3],[7,9]],sliced.values)
        npt.assert_array_equal(["record1","record3"],sliced.index)
        npt.assert_array_equal(["a","c"],sliced.columns)

    def test_iloc1(self):
        """
        'i' stands for integer-position-based indexing and slicing
        """
        frame = pd.DataFrame(np.arange(1,10).reshape(3,3),
                             columns = ["a","b","c"],
                             index = ["record1","record2","record3"])

        # I don't think integer-position-based column indexing is useful
        # so only demonstrate integer-position-based row indexing
        subframe = frame.iloc[[2,1]]

        expected = pd.DataFrame([[7, 8, 9],       
                                 [4, 5, 6]],columns=frame.columns,index = ["record3","record2"])

        pdt.assert_frame_equal(expected,subframe,check_dtype=False)

    def test_ix_mix_label_position(self):
        df = pd.DataFrame(np.arange(1,10).reshape(3,3),
                             columns = ["a","b","c"],
                             index = ["record1","record2","record3"])
        npt.assert_equal([[1, 2, 3],       
                          [4, 5, 6],       
                          [7, 8, 9]],df.values)

        pdt.assert_frame_equal(pd.DataFrame([[1, 3],          
                                             [7, 9]],columns=["a","c"],index=["record1","record3"]),
                               df.ix[[0,2],["a","c"]],
                               check_dtype=False)

    
    def test_ix1(self):
        """
        ix supports mixed integer and label based access. But, it is primarily label based.
        """
        frame = pd.DataFrame(np.arange(1,10).reshape(3,3),
                             columns = ["a","b","c"],
                             index = ["record1","record2","record3"])
        npt.assert_equal([[1, 2, 3],       
                          [4, 5, 6],       
                          [7, 8, 9]],frame.values)

        # ------------------- rows
        npt.assert_equal([[7, 8, 9],       
                          [1, 2, 3]], frame.ix[["record3","record1"]].values)

        # ------------------- columns
        npt.assert_equal([[3, 2],       
                          [6, 5],       
                          [9, 8]], frame.ix[:,["c","b"]].values)

        # ------------------- sub-frame
        sliced_copy = frame.ix[["record1","record2"],
                               ["a","b"]]
        npt.assert_equal([[1, 2],       
                          [4, 5]], sliced_copy.values)
        
        sliced_copy *= -1
        npt.assert_equal([[-1, -2],       
                          [-4, -5]], sliced_copy.values)

        # !!!  won't affect the original
        npt.assert_equal([[1, 2, 3],       
                          [4, 5, 6],       
                          [7, 8, 9]],frame.values)

    def test_select_single_row(self):
        frame = pd.DataFrame(np.arange(1,10).reshape(3,3),
                             columns = ["a","b","c"],
                             index = ["record1","record2","record3"])
        npt.assert_equal([[1, 2, 3],       
                          [4, 5, 6],       
                          [7, 8, 9]],frame.values)

        # ---------- use ix or loc, and pass in the index name
        row_series_copy = frame.ix["record1"]
        self.assertIsInstance(row_series_copy,pd.Series)

        row_series_copy *= -1
        npt.assert_equal([-1,-2,-3],row_series_copy.values)

        # ---------- original isn't affected
        npt.assert_equal([[1, 2, 3],       
                          [4, 5, 6],       
                          [7, 8, 9]],frame.values)

    def test_xs_view(self):
        df = pd.DataFrame(np.arange(1,10).reshape(3,3),
                             columns = ["a","b","c"],
                             index = ["record1","record2","record3"])
        npt.assert_equal([[1, 2, 3],       
                          [4, 5, 6],       
                          [7, 8, 9]],df.values)
        # ***************** single row
        row_view = df.xs("record2")
        npt.assert_equal([4,5,6],row_view.values)

        # !!!  change view will affect the original
        row_view.a *= -1
        npt.assert_equal([-4,5,6],row_view.values)

        npt.assert_equal([[1, 2, 3],       
                          [-4, 5, 6],       
                          [7, 8, 9]],df.values)# original frame is also changed

        # ***************** single column
        col_view = df.xs("c",axis=1)
        npt.assert_equal([3,6,9],col_view.values)

        col_view.record1 *= -1
        npt.assert_equal([-3,6,9],col_view.values)

        npt.assert_equal([[1, 2, -3],       
                          [-4, 5, 6],       
                          [7, 8, 9]],df.values)# original frame is also changed


    def test_select_single_cell(self):
        df = pd.DataFrame(np.arange(1,5).reshape((2,2)),index=["r1","r2"],columns=["c1","c2"])

        # use attribute
        self.assertEqual(2,df.c2.r1)

        # use loc/ix
        self.assertEqual(1, df.loc["r1","c1"])

