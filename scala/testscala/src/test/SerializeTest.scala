package test

import java.io.ByteArrayInputStream
import java.io.ByteArrayOutputStream
import java.io.ObjectInputStream
import java.io.ObjectOutputStream
import org.scalatest.Spec

sealed class SerializeTest extends Spec {

  def serializeCopy[T](src: T) = {
    val byteOutStream = new ByteArrayOutputStream
    val objOutStream = new ObjectOutputStream(byteOutStream)
    objOutStream.writeObject(src)
    // !!! must close, otherwise, it won't flush 
    // !!! JVM cache even when we are writing to byte array
    objOutStream.close()

    val byteInStream = new ByteArrayInputStream(byteOutStream.toByteArray)
    val objInStream = new ObjectInputStream(byteInStream)
    val cpy = objInStream.readObject()
    objInStream.close()

    cpy.asInstanceOf[T]
  } //def

  object `incorrect reset` {

    sealed class WrongReusableSerializer {
      private val _bytestream = new ByteArrayOutputStream
      private val _objstream = new ObjectOutputStream(_bytestream)

      def close() = _objstream.close()

      def serialize(dest: Array[Byte], dstoffset: Int, src: Any) = {
        _objstream.reset() // XXXXXXXXXXX reset will change the underlying stream
        _objstream.writeObject(src)
        _objstream.flush()

        val bytearray = _bytestream.toByteArray
        bytearray.copyToArray(dest, dstoffset)

        dstoffset + bytearray.length
      } //def 

    } //class

    def `incorrect reset 1` = {
      val byteOutStream = new ByteArrayOutputStream
      val objOutStream = new ObjectOutputStream(byteOutStream)

      /*
       * !!! the problem is: 
       * !!! you cannot reset the ByteArrayStream after delegating it to ObjectOutputStream
       * !!! you can reset the ObjectOutputStream, but cannot reset the underlying stream
       * !!! I guess, after delegating the underlying stream to the ObjectOutputStream
       * !!! ObjectOutputStream has already written some header information into the underlying stream
       * !!! reset the underlying stream, will erase those header information
       * !!! which cause the error
       */
      byteOutStream.reset()
      objOutStream.writeInt(999)
      objOutStream.close()

      val byteInStream = new ByteArrayInputStream(byteOutStream.toByteArray)
      intercept[java.io.StreamCorruptedException] { val objInStream = new ObjectInputStream(byteInStream) }
    } //def

    def `incorrect reset 2` = {
      val bytearray = new Array[Byte](128)
      val serializer = new WrongReusableSerializer

      var offset = 0
      offset = serializer.serialize(bytearray, offset, "stasi")
      offset = serializer.serialize(bytearray, offset, "cheka")

      val byteInStream = new ByteArrayInputStream(bytearray)
      val objInStream = new ObjectInputStream(byteInStream)
      val txt1 = objInStream.readObject.asInstanceOf[String]
      intercept[java.io.StreamCorruptedException] { objInStream.readObject.asInstanceOf[String] }
    } //def

  } //object

  object `test basic types` {

    def `test primitive type` = {
      assert(serializeCopy(9) == 9)
      assert(serializeCopy("stasi") == "stasi")
    } //def

    def `test collections` = {

      assert(serializeCopy(Seq(1, 2, 3)) == List(1, 2, 3))

      val srcArray = Array(7, 8, 9)
      val cpyArray = serializeCopy(srcArray)
      assert(srcArray sameElements cpyArray)

    } //def

    def `test specific type` = {
      // --------------------------------------- write
      val byteoutstream = new ByteArrayOutputStream
      val objoutstream = new ObjectOutputStream(byteoutstream)

      val array = Array(1, 6, 8, 9)
      array.foreach(objoutstream.writeInt(_))
      objoutstream.close() // !!! otherwise, it won't flush, even when written to byte array

      // --------------------------------------- read
      val byteinstream = new ByteArrayInputStream(byteoutstream.toByteArray)
      val objinstream = new ObjectInputStream(byteinstream)
      val cpyarray = for (index <- 1 to array.length) yield objinstream.readInt
      objinstream.close()

      // --------------------------------------- check
      assert(array sameElements cpyarray)
    }
  } //object

}//class