package tests

import org.scalatest.Spec

/*
 * view and stream are both lazy-evaluated data structures
 * the difference between them:
 * 1. view are always lazy-evaluated for all its elements. however, stream is active for its head, but lazy for its tail part
 * 2. stream cache all its previous-accessed elements. while, view has no memory, always re-generate
 */
sealed class ViewTest extends Spec {

  object `demo how view works` {

    def `view has no cache` = {

      var sideeffect = 0
      def mimicExpensiveOperation(x: Int) = {
        sideeffect += 1 // mimic a heavy computation to get a result
        x * 2
      }

      val originalCollection = 1 to 10
      val resultView = (originalCollection.view) map mimicExpensiveOperation
      assertResult(0)(sideeffect)

      assertResult(2)(resultView(0))
      assertResult(1)(sideeffect)

      // even access the same element again, since no cache
      // the element will be generated again, so side effect again
      assertResult(2)(resultView(0))
      assertResult(2)(sideeffect)
      
      // view only perform necessary calculations, won't waste calculation on elements which aren't needed
      assertResult(4)(resultView(1))
      assertResult(3)(sideeffect)

      // call force to convert back to a concrete collection
      val resultCollection = resultView.force
      assertResult(2 to 20 by 2)(resultCollection)
      assertResult(13)(sideeffect)
    } //def

  } //object

}