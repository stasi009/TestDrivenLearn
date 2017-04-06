package tests

import org.scalatest.Spec

sealed class ExtractorTest extends Spec {

  object Student {
    def apply(id: Int, name: String, sex: String, addr: String) = s"$id,$name,$sex,$addr"

    def unapply(msg: String) = {
      val parts = msg.split(",")
      if (parts.length == 4) Some(parts(0).toInt, parts(1), parts(2), parts(3))
      else None
    } //def
  } //extractor object

  object IsMale {
    // return Boolean (other than Optional) to indicate "matched or not"
    def unapply(s: String) = s.toLowerCase == "male"
  }

  object Name {
    def apply(firstname: String, lastname: String) = s"$firstname $lastname"

    def unapply(fullname: String) = {
      val segments = fullname split " "
      if (segments.length == 2) Some(segments(0), segments(1))
      else None
    }
  }

  def `injection method` = {
    // ------------- inject
    val firstname = "stasi"
    val lastname = "kgb"
    val fullname = Name(firstname, lastname)

    // ------------- extract
    val Name(cpyfirst, cpylast) = fullname
    assert(cpyfirst == firstname)
    assert(cpylast == lastname)
  }

  def `demo 1` = {
    val Name(firstname, lastname) = "stasi cheka"
    assert(firstname == "stasi")
    assert(lastname == "cheka")

    intercept[MatchError] { val Name(_, _) = "stasi" }
  }

  def `used in pattern match` = {
    def isValidName(name: String) = {
      name match {
        case Name(_, _) => true
        case _ => false
      } //match
    } //def
    assertResult(Seq(true, false, true)) { Seq("stasi kgb", "x", "gru cheka") map isValidName }
  }

  // it will automatically filter out those not matched
  def `for loop will ignore not-matched` = {
    val students = Seq(
      "11,Justin,male,Kaohsiung",
      "yyyyyyyyyyyyy",
      "98,Monica,female,Kaohsiung",
      "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
      "66,Bush,male,Taipei")
    val ids = for (Student(id, name, sex, addr) <- students) yield id
    assert(ids == Seq(11, 98, 66))
  } //def

  def `use extractor to judge matched or not` = {
    val students = Seq(
      "11,Justin,male,Kaohsiung",
      "yyyyyyyyyyyyy",
      "98,Monica,female,Kaohsiung",
      "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
      "66,Bush,male,Taipei")

    val ids = for (Student(id, _, sex @ IsMale(), _) <- students) yield id
    assert(ids == Seq(11, 66))
  } //def

  def `use in partial function` = {
    val students = Seq(
      "11,Justin,male,Kaohsiung",
      "yyyyyyyyyyyyy",
      "98,Monica,female,Kaohsiung",
      "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
      "66,Bush,male,Taipei")

    // partial function, ignore those not matched
    val allids = students collect { case Student(id, _, _, _) => id }
    assert(allids == Seq(11, 98, 66))

    val maleids = students collect { case Student(id, _, sex @ IsMale(), _) => id }
    assert(maleids == Seq(11, 66))
  }

}