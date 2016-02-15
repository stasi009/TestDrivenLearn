
"""
io.StringIO requires unicode
it can be used with "with" statement

while StringIO.StringIO just need ordinary string
however, StringIO.StringIO cannot be used with "with" statement
"""

import unittest
import StringIO
import cStringIO

class StringIOTest(unittest.TestCase):
    """
    string Concatenation in Python
    Heavy use of StringIO.StringIO objects can be made more efficient by using the function StringIO() from this module instead.
    """

    def test_stringio(self):
        sio = cStringIO.StringIO()

        sio.write("hello python")
        sio.write(" from sifang")
        self.assertEqual("hello python from sifang",sio.getvalue())
            
        print >> sio, " in Beijing" # concatenate
        self.assertEqual("hello python from sifang in Beijing\n",sio.getvalue())
            
        sio.close()
        with self.assertRaises(ValueError): # io operation on closed file
            sio.getvalue()
            
                
    def test_multiple_getvalue(self):
        """StringIO.getvalue can be invoked multiple times"""
        sio = cStringIO.StringIO()

        sio.write("hello python")
        sio.write(" from sifang")
        self.assertEqual("hello python from sifang",sio.getvalue())
        # invoke another time
        self.assertEqual("hello python from sifang",sio.getvalue())

        sio.close()
        
            
            
            
        
        
        
    
    