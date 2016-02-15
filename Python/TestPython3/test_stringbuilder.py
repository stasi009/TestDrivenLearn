
import unittest
import io

########################################################################
class StringBuilderTest(unittest.TestCase):
    """string Concatenation in Python"""

    def test_stringio(self):
        with io.StringIO() as sio:
            sio.write("hello python")
            sio.write(" from sifang")
            self.assertEqual("hello python from sifang",sio.getvalue())
            
            print(" in Beijing",file=sio) # print will add the "new line" character
            self.assertEqual("hello python from sifang in Beijing\n",sio.getvalue())
            
            sio.close()
            with self.assertRaises(ValueError): # io operation on closed file
                sio.getvalue()
                
    def test_multiple_getvalue(self):
        """StringIO.getvalue can be invoked multiple times"""
        with io.StringIO() as sio:
            sio.write("hello python")
            sio.write(" from sifang")
            self.assertEqual("hello python from sifang",sio.getvalue())
            # invoke another time
            self.assertEqual("hello python from sifang",sio.getvalue())
        
            
            
            
        
        
        
    
    