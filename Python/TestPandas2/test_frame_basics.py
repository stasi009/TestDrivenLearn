
import unittest
import numpy as np
import numpy.testing as npt
import pandas as pd
import pandas.util.testing as pdt
import test_testing as tt

class FrameBasicsTest(unittest.TestCase):

    def test_copy_constructor(self):
        original = pd.DataFrame(np.arange(1,5).reshape((2,2)),index=["r1","r2"],columns=["c1","c2"])

        # ------------------ sub frame, this time return a copy
        subcopy = pd.DataFrame(original,columns=["c2"])
        subcopy.loc["r1","c2"] = -99
        npt.assert_equal([[-99],[4]],subcopy.values)

        # original isn't changed
        npt.assert_equal([[1,2],
                          [3,4]],original.values)

        # ------------------ pass whole without slicing, then return view
        view = pd.DataFrame(original)

        view.c1.r1 = -100
        npt.assert_equal([[-100,2],
                          [3,4]],view.values)
        npt.assert_equal([[-100,2],
                          [3,4]],original.values)


    def test_create_demo(self):
        # provide both columns and "row names"
        frame = pd.DataFrame(np.arange(1,10).reshape(3,3),
                             columns = ["a","b","c"],
                             index = ["record1","record2","record3"])
        expected = pd.Series([2,5,8],index=frame.index)
        pdt.assert_series_equal(expected,frame.b,check_dtype=False,check_names=False)

    def test_create_from_dict(self):
        data = {'state': ['Ohio', 'Ohio', 'Ohio', 'Nevada', 'Nevada'],
                'year': [2000, 2001, 2002, 2001, 2002],
                'pop': [1.5, 1.7, 3.6, 2.4, 2.9]}
        frame1 = pd.DataFrame(data)
        # each outer key becomes a column
        npt.assert_array_equal(["pop","state","year"],frame1.columns)

        # another way to create frame from dict
        # you can specify the columns, this way not only specify the order of the columns
        # but also, if you specify a column which doesn't exist in the original dict
        # that column will be all NAN
        frame2 = pd.DataFrame(data,columns=["year","state","pop","debt"])

        # "debt" is a column not exist in dict, so it will be all NAN
        self.assertTrue(np.all(frame2.debt.isnull()))

    def test_index1(self):
        data = {"b":[3,4],
                "c":[5,6],
                "a":[1,2]}
        frame = pd.DataFrame(data)
        
        # columns become attributes
        npt.assert_equal([3,4], frame.b.values)

        # multiple columns
        expected = pd.DataFrame({"c":[5,6],
                                  "a":[1,2]})
        actual = frame[["a","c"]] # column matters
        pdt.assert_frame_equal(expected,actual)

    def test_ix1(self):
        frame = pd.DataFrame(np.arange(1,10).reshape(3,3),
                             columns = ["a","b","c"],
                             index = ["record1","record2","record3"])
        subframe = frame.ix[["record3","record1"],["c","a"]]

        expected = pd.DataFrame({"a":{"record1":1,"record3":7},
                                 "c":{"record1":3,"record3":9}})
        tt.ignore_order_assert_frames_equal(expected,subframe)

    def test_head_tail(self):
        df = pd.DataFrame(np.arange(1,10).reshape(3,3),
                             columns = ["a","b","c"],
                             index = ["record1","record2","record3"])

        # by default, head, tail will show 5 rows
        pdt.assert_frame_equal(pd.DataFrame([[4, 5, 6],       
                                             [7, 8, 9]],columns=df.columns,index=["record2","record3"]),df.tail(2),check_dtype=False)