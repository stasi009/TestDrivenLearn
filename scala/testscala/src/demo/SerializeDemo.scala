package demo

import java.io.ByteArrayInputStream
import java.io.ByteArrayOutputStream
import java.io.ObjectInputStream
import java.io.ObjectOutputStream

object SerializeDemo extends App {

  def serialize(src: Any) = {
    val byteOutStream = new ByteArrayOutputStream

    val objOutStream = new ObjectOutputStream(byteOutStream)
    objOutStream.writeObject(src)
    objOutStream.close()

    byteOutStream.toByteArray
  } //def

  def serializeArray() = {
    val src = Array(1 to 1000: _*)
    val bytearray = serialize(src)
    println(s"${src.length} integers serialize to ${bytearray.length} bytes")

    // each integer occupies 4 bytes
    val expected = 4 * src.length + 27
    assert(bytearray.length == expected)
  } //def

  // inspect the size after the serialization
  def checkSize() = {
    /*
     * write int as object, will cost a lot of space
     * because it will cause boxing, which will increase the space overhead
     * the correct way to serialize int, should call ObjectOutpputStream.writeInt method
     */
    println("---------------- integers")
    val intarray = Array(1, 10, 100, 1000, 10000, 11, 345, 87368)
    for (i <- intarray) {
      val bytearray = serialize(i)
      println(s"$i: ${bytearray.length} bytes")
    } //for

    println("---------------- strings")
    val strarray = Array("a", "bcd", "xyzekd", "yyyyyyyyyyxz")
    for (s <- strarray) {
      val bytearray = serialize(s)
      println(s"$s: ${s.length} characters, ${bytearray.length} bytes")
    } //for
  }

  def closeToFlush() = {
    val byteOutStream = new ByteArrayOutputStream
    val objOutStream = new ObjectOutputStream(byteOutStream)
    objOutStream.writeInt(99)
    objOutStream.writeInt(66)
    // !!! pay attention that, "close" is very very important
    // !!! stupid JVM cache even when the underlying stream is backed by byte array
    // !!! you have to close to flush
    objOutStream.close()

    val bytearray = byteOutStream.toByteArray
    println(s"${bytearray.length} bytes")

    val byteInStream = new ByteArrayInputStream(bytearray)
    val objInStream = new ObjectInputStream(byteInStream)
    assert(objInStream.readInt == 99)
    assert(objInStream.readInt == 66)
    objInStream.close()
  }

  override def main(args: Array[String]) = {
    // serializeArray()
    // checkSize()
    closeToFlush()
  } //def

}