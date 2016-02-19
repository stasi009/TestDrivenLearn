package wsu.cheka.basictest;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertFalse;
import static org.junit.Assert.assertTrue;

import org.junit.Test;

public class BaseExtendTest {
	private enum InvokeVersion {
		INVOKE_PARENT, INVOKE_CHILD, INVOKE_UNDEFINE
	}

	private static class CallerObject {
		private InvokeVersion m_invokeVersion = InvokeVersion.INVOKE_UNDEFINE;

		public InvokeVersion getInvokeVersion() {
			return m_invokeVersion;
		}

		public void function(Parent obj) {
			m_invokeVersion = InvokeVersion.INVOKE_PARENT;
		}

		public void function(Child obj) {
			m_invokeVersion = InvokeVersion.INVOKE_CHILD;
		}
	}

	private static class CalledParm {
		private InvokeVersion m_callerFlag = InvokeVersion.INVOKE_UNDEFINE;

		public void setCaller(InvokeVersion callerFlag) {
			m_callerFlag = callerFlag;
		}

		public InvokeVersion getCaller() {
			return m_callerFlag;
		}
	}

	private static class Parent {
		/**
		 * in Java, different from C++ or C# no "virtual" keyword is needed
		 */
		protected boolean m_flag;

		public Parent() {
			m_flag = true;
		}

		public void publicAction(CalledParm parm) {
			privateAction(parm);
		}

		protected void protectedAction(CalledParm parm) {
			privateAction(parm);
		}

		protected final void finalAction(CalledParm parm) {
			privateAction(parm);
		}

		private void privateAction(CalledParm parm) {
			parm.setCaller(InvokeVersion.INVOKE_PARENT);
		}

		public Parent MakeNewObject() {
			return new Parent();
		}
	}

	/**
	 * all the method which can be accessed outside the object can be override,
	 * which indicates that "private" methods must always be final, which can
	 * not be override but private methods are always invisible, whether can be
	 * overriden or not has no meaning to these methods
	 */
	private static class Child extends Parent {
		public Child() {
		}

		/**
		 * different from C#, no "override" keyword is needed
		 */
		public void publicAction(CalledParm parm) {
			privateAction(parm);
		}

		protected void protectedAction(CalledParm parm) {
			privateAction(parm);
		}

		/*
		 * if a method in base class is marked as "final" then a method with the
		 * same signature existing in the derived class will be deemed as a
		 * error by the compiler protected void finalAction(CalledParm parm) {
		 * parm.setCaller(InvokeVersion.INVOKE_CHILD); }
		 */

		/**
		 * although this method has the same signature with the base class
		 * version but has no relationship with the base class verion because
		 * it's private, has no access privilege to each other
		 */
		private void privateAction(CalledParm parm) {
			parm.setCaller(InvokeVersion.INVOKE_CHILD);
		}

		/**
		 * the feature that the override method in child class can return a
		 * derived type actually has very little meaning the only advantage, in
		 * my opinion, is that if this method is called without a polymorphism
		 * environment then this method can return the exact type needed no
		 * extra cast is required.
		 */
		@Override
		public Child MakeNewObject() {
			return new Child();
		}
	}

	/**
	 * test that the compiler will always find the most matched method to be
	 * called
	 * !!!!!!!!!!!!!!!!!!!!!!!!!!!! 
	 * this test shows important feature of overload
	 * that is, Overload is static naming binding
	 * Java uses static binding for overloaded methods, and dynamic binding for overridden ones
	 * !!!!!!!!!!!!!!!!!!!!!!!!!!!! 
	 */
	@Test
	public void testOverloadResolution() {
		Child childObj = new Child();

		// test the compiler will find the most matched version
		CallerObject callObj = new CallerObject();
		callObj.function(childObj);
		assertEquals(InvokeVersion.INVOKE_CHILD, callObj.getInvokeVersion());

		// if we want the parent version to be called
		// explicit cast is necessary
		callObj.function((Parent) childObj);
		assertEquals(InvokeVersion.INVOKE_PARENT, callObj.getInvokeVersion());
	}

	interface IInvoke {
		void action(Parent obj, CalledParm parm);
	}

	static class InvokePublic implements IInvoke {
		public void action(Parent obj, CalledParm parm) {
			obj.publicAction(parm);
		}
	}

	static class InvokeProtected implements IInvoke {
		public void action(Parent obj, CalledParm parm) {
			// can be called within the same package
			obj.protectedAction(parm);
		}
	}

	private void testOverride(IInvoke invoker) {
		CalledParm parm = new CalledParm();
		Parent parentObj = new Parent();
		Child childObj = new Child();

		Parent generalRef = parentObj;
		invoker.action(generalRef, parm);
		assertEquals(InvokeVersion.INVOKE_PARENT, parm.getCaller());

		generalRef = childObj;
		invoker.action(generalRef, parm);
		assertEquals(InvokeVersion.INVOKE_CHILD, parm.getCaller());
	}

	@Test
	public void testPublicOverride() {
		testOverride(new InvokePublic());
	}

	@Test
	public void testProtectedOverride() {
		testOverride(new InvokeProtected());
	}

	/**
	 * test that in Java, override function has different return type, as long
	 * as the return object by overriden method derives from the return type by
	 * base class
	 */
	@Test
	public void testOverrideDerivedReturnType() {
		Parent parentobj = new Parent();
		Child childobj = new Child();
		Parent baseref = null;

		baseref = parentobj;
		Parent newobj = baseref.MakeNewObject();
		assertFalse(newobj instanceof Child);

		baseref = childobj;
		newobj = baseref.MakeNewObject();
		assertTrue(newobj instanceof Child);
	}

	/**
	 * test that base class's default constructor will be automatically be
	 * invoked
	 */
	@Test
	public void testBaseDefaultConsInvoked() {
		Child childobj = new Child();
		assertTrue(
				"parent's default constructor has been automatically be invoked",
				childobj.m_flag);
	}

	@Test
	public void testIsInstanceOf() {
		Child childObj = new Child();
		assertTrue(childObj instanceof Parent);
	}
}
