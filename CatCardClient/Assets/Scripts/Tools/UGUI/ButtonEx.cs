using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Z.Frame;

namespace Z.Tool
{
	[AddComponentMenu("UGUIExpend/ButtonEx", 30)]
	public class ButtonEx : Selectable, IPointerClickHandler,IOnUpdate
	{
		protected ButtonEx() { }

		[Serializable]
		public class ButtonExEvent : UnityEvent
		{
			public PointerEventData eventData = default;
		}

		[SerializeField]
		private ButtonExEvent m_OnDown = new ButtonExEvent();

		[SerializeField]
		private ButtonExEvent m_OnUp = new ButtonExEvent();

		[SerializeField]
		private ButtonExEvent m_OnEnter = new ButtonExEvent();

		[SerializeField]
		private ButtonExEvent m_OnExit = new ButtonExEvent();

		[SerializeField]
		private ButtonExEvent m_OnClick = new ButtonExEvent();

		[SerializeField]
		private ButtonExEvent m_OnLongPress = new ButtonExEvent();

		[SerializeField]
		private ButtonExEvent m_OnDoubleClick = new ButtonExEvent();

		public ButtonExEvent OnDown
		{
			get { return this.m_OnDown; }
			set { this.m_OnDown = value; }
		}
		public ButtonExEvent OnUp
		{
			get { return this.m_OnUp; }
			set { this.m_OnUp = value; }
		}
		public ButtonExEvent OnEnter
		{
			get { return this.m_OnEnter; }
			set { this.m_OnEnter = value; }
		}
		public ButtonExEvent OnExit
		{
			get { return this.m_OnExit; }
			set { this.m_OnExit = value; }
		}
		public ButtonExEvent OnClick
		{
			get { return this.m_OnClick; }
			set { this.m_OnClick = value; }
		}
		public ButtonExEvent OnLongPress
		{
			get { return this.m_OnLongPress; }
			set { this.m_OnLongPress = value; }
		}
		public ButtonExEvent OnDoubleClick
		{
			get { return this.m_OnDoubleClick; }
			set { this.m_OnDoubleClick = value; }
		}

		private bool isEnter = false;
		private bool isClick = false;//是否点击了
		private bool isPointDown = false;
		private float time = 0;//按下时间
		private float interval = 2f;//长按判断时间

		protected override void OnDestroy()
		{
			base.OnDestroy();
			UpdateManager.Remove(this);
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			base.OnPointerDown(eventData);
			this.isPointDown = true;
			this.time = 0;
			m_OnDown?.Invoke();

			UpdateManager.Add(this);
		}

		public override void OnPointerEnter(PointerEventData eventData)
		{
			base.OnPointerEnter(eventData);
			if (isClick)
				return;

			isEnter = true;
			m_OnEnter?.Invoke();
		}

		public override void OnPointerUp(PointerEventData eventData)
		{
			base.OnPointerUp(eventData);
			this.isPointDown = false;
			if (!isEnter)
				m_OnUp?.Invoke();
			else
				isClick = true;
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			base.OnPointerExit(eventData);
			if (isClick)
				return;

			isPointDown = false;
			isEnter = false;
			m_OnExit?.Invoke();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			clickCount++;
			if (eventData.clickCount == 1)
			{
				if (isClick)
				{
					m_OnClick?.Invoke();
					isClick = false;
				}
			}
		}

		private int clickCount = 0;
		private float clickTime;
		private float doubleClickInterval = 0.2f;
		private void OnDoubleClickEvent()
		{
			m_OnDoubleClick?.Invoke();
		}

		public float OnUpdate()
		{
			if(clickCount >= 1)
			{
				clickTime += Time.deltaTime;
				if(clickTime <= doubleClickInterval && clickCount == 2)
				{
					clickCount = 0;
					clickTime = 0;
					OnDoubleClickEvent();
				}

				if (clickCount > 2 || clickTime > doubleClickInterval)
				{
					clickCount = 0;
					clickTime = 0;
				}
			}

			if (isPointDown)
			{
				this.time += Time.deltaTime;
				if(this.time >= this.interval)
				{
					isPointDown = false;
					m_OnLongPress?.Invoke();
					return -1;
				}
			}

			return 0;
		}
	}
}

