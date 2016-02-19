package wsu.cheka.basictest;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertSame;
import static org.junit.Assert.assertTrue;

import java.util.Observable;
import java.util.Observer;
import java.util.Random;

import org.junit.Before;
import org.junit.Test;

public class ObserverTest {
	/**
	 * event argument
	 */
	private static class PriceChangedEvent {
		private int m_oldprice;
		private int m_newprice;

		public PriceChangedEvent(int oldprice, int newprice) {
			m_oldprice = oldprice;
			m_newprice = newprice;
		}

		int getOldPrice() {
			return m_oldprice;
		}

		int getNewPrice() {
			return m_newprice;
		}
	}

	/**
	 * event publisher
	 */
	private static class Stock extends Observable {
		private String m_name;
		private int m_price;

		public Stock(String name, int price) {
			m_name = name;
			m_price = price;
		}

		public int getPrice() {
			return m_price;
		}

		public void setPrice(int newPrice) {
			if (newPrice != m_price) {
				int oldprice = m_price;
				m_price = newPrice;

				setChanged();
				notifyObservers(new PriceChangedEvent(oldprice, m_price));
			}
		}
	}

	private int m_oldprice;
	private int m_newprice;
	private Stock m_stock;
	private boolean m_isEvtFired;

	@Before
	public void setUp() {
		Random rand = new Random(System.currentTimeMillis());

		m_oldprice = rand.nextInt();
		m_newprice = m_oldprice + 2;// assure that m_newprice is
		// different from m_oldprice

		m_isEvtFired = false;

		m_stock = new Stock("test", m_oldprice);
		m_stock.addObserver(new Observer() {
			@Override
			public void update(Observable o, Object arg) {
				OnChangeSetFlag();
			}
		});
		m_stock.addObserver(new Observer() {
			@Override
			public void update(Observable o, Object arg) {
				OnChangeAssertPrice(o, arg);
			}
		});
	}

	private void OnChangeSetFlag() {
		m_isEvtFired = true;
	}

	private void OnChangeAssertPrice(Observable source, Object evtargs) {
		assertSame(m_stock, source);

		PriceChangedEvent changedEvent = (PriceChangedEvent) evtargs;
		assertEquals(m_oldprice, changedEvent.getOldPrice());
		assertEquals(m_newprice, changedEvent.getNewPrice());
	}

	@Test
	public void testObserverPattern() {
		m_stock.setPrice(m_newprice);

		assertTrue(m_isEvtFired);
		assertEquals(m_newprice, m_stock.getPrice());
	}
}
