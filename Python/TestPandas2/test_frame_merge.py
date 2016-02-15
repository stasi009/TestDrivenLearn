
import unittest
import numpy as np
import numpy.testing as npt
import pandas as pd
import pandas.util.testing as pdt

class FrameMergeTest(unittest.TestCase):

    def test_demo1(self):
        df1 = pd.DataFrame({'key': ['b', 'b', 'a', 'c', 'a', 'a', 'b'],'data1': [1 + i * 0.1 for i in xrange(1,8)]})
        df2 = pd.DataFrame({'key': ['a', 'b', 'd'],  'data2': [2.1,2.2,2.3]})

        # If not specified, merge uses the overlapping column names as the keys. It's a good practice to specify explicitly, though
        merged = pd.merge(df1,df2,on="key")