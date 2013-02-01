using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace Tmc.SystemFrameworks.Common
{
    //#region version 1
    //public class VirtualizingWrapPanel : VirtualizingPanel, IScrollInfo
    //{

    //    #region Fields

    //    UIElementCollection _children;
    //    ItemsControl _itemsControl;
    //    IItemContainerGenerator _generator;
    //    private Point _offset = new Point(0, 0);
    //    private Size _extent = new Size(0, 0);
    //    private Size _viewport = new Size(0, 0);
    //    private int _firstIndex = 0;
    //    private Size _childSize;
    //    private Size _pixelMeasuredViewport = new Size(0, 0);
    //    Dictionary<UIElement, Rect> _realizedChildLayout = new Dictionary<UIElement, Rect>();
    //    WrapPanelAbstraction _abstractPanel;


    //    #endregion

    //    #region Properties

    //    private Size ChildSlotSize
    //    {
    //        get
    //        {
    //            return new Size(ItemWidth, ItemHeight);
    //        }
    //    }

    //    #endregion

    //    #region Dependency Properties

    //    [TypeConverter(typeof(LengthConverter))]
    //    public double ItemHeight
    //    {
    //        get
    //        {
    //            return (double)base.GetValue(ItemHeightProperty);
    //        }
    //        set
    //        {
    //            base.SetValue(ItemHeightProperty, value);
    //        }
    //    }

    //    [TypeConverter(typeof(LengthConverter))]
    //    public double ItemWidth
    //    {
    //        get
    //        {
    //            return (double)base.GetValue(ItemWidthProperty);
    //        }
    //        set
    //        {
    //            base.SetValue(ItemWidthProperty, value);
    //        }
    //    }

    //    public Orientation Orientation
    //    {
    //        get { return (Orientation)GetValue(OrientationProperty); }
    //        set { SetValue(OrientationProperty, value); }
    //    }

    //    public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register("ItemHeight", typeof(double), typeof(VirtualizingWrapPanel), new FrameworkPropertyMetadata(double.PositiveInfinity));
    //    public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register("ItemWidth", typeof(double), typeof(VirtualizingWrapPanel), new FrameworkPropertyMetadata(double.PositiveInfinity));
    //    public static readonly DependencyProperty OrientationProperty = StackPanel.OrientationProperty.AddOwner(typeof(VirtualizingWrapPanel), new FrameworkPropertyMetadata(Orientation.Horizontal));

    //    #endregion

    //    #region Methods

    //    public void SetFirstRowViewItemIndex(int index)
    //    {
    //        SetVerticalOffset((index) / Math.Floor((_viewport.Width) / _childSize.Width));
    //        SetHorizontalOffset((index) / Math.Floor((_viewport.Height) / _childSize.Height));
    //    }

    //    private void Resizing(object sender, EventArgs e)
    //    {
    //        if (_viewport.Width != 0)
    //        {
    //            int firstIndexCache = _firstIndex;
    //            _abstractPanel = null;
    //            MeasureOverride(_viewport);
    //            SetFirstRowViewItemIndex(_firstIndex);
    //            _firstIndex = firstIndexCache;
    //        }
    //    }

    //    public int GetFirstVisibleSection()
    //    {
    //        int section;
    //        var maxSection = _abstractPanel.Max(x => x.Section);
    //        if (Orientation == Orientation.Horizontal)
    //        {
    //            section = (int)_offset.Y;
    //        }
    //        else
    //        {
    //            section = (int)_offset.X;
    //        }
    //        if (section > maxSection)
    //            section = maxSection;
    //        return section;
    //    }

    //    public int GetFirstVisibleIndex()
    //    {
    //        int section = GetFirstVisibleSection();
    //        var item = _abstractPanel.Where(x => x.Section == section).FirstOrDefault();
    //        if (item != null)
    //            return item._index;
    //        return 0;
    //    }

    //    private void CleanUpItems(int minDesiredGenerated, int maxDesiredGenerated)
    //    {
    //        for (int i = _children.Count - 1; i >= 0; i--)
    //        {
    //            GeneratorPosition childGeneratorPos = new GeneratorPosition(i, 0);
    //            int itemIndex = _generator.IndexFromGeneratorPosition(childGeneratorPos);
    //            if (itemIndex < minDesiredGenerated || itemIndex > maxDesiredGenerated)
    //            {
    //                _generator.Remove(childGeneratorPos, 1);
    //                RemoveInternalChildRange(i, 1);
    //            }
    //        }
    //    }

    //    private void ComputeExtentAndViewport(Size pixelMeasuredViewportSize, int visibleSections)
    //    {
    //        if (Orientation == Orientation.Horizontal)
    //        {
    //            _viewport.Height = visibleSections;
    //            _viewport.Width = pixelMeasuredViewportSize.Width;
    //        }
    //        else
    //        {
    //            _viewport.Width = visibleSections;
    //            _viewport.Height = pixelMeasuredViewportSize.Height;
    //        }

    //        if (Orientation == Orientation.Horizontal)
    //        {
    //            _extent.Height = _abstractPanel.SectionCount + ViewportHeight - 1;

    //        }
    //        else
    //        {
    //            _extent.Width = _abstractPanel.SectionCount + ViewportWidth - 1;
    //        }
    //        _owner.InvalidateScrollInfo();
    //    }

    //    private void ResetScrollInfo()
    //    {
    //        _offset.X = 0;
    //        _offset.Y = 0;
    //    }

    //    private int GetNextSectionClosestIndex(int itemIndex)
    //    {
    //        var abstractItem = _abstractPanel[itemIndex];
    //        if (abstractItem.Section < _abstractPanel.SectionCount - 1)
    //        {
    //            var ret = _abstractPanel.
    //                Where(x => x.Section == abstractItem.Section + 1).
    //                OrderBy(x => Math.Abs(x.SectionIndex - abstractItem.SectionIndex)).
    //                First();
    //            return ret._index;
    //        }
    //        else
    //            return itemIndex;
    //    }

    //    private int GetLastSectionClosestIndex(int itemIndex)
    //    {
    //        var AbstractItem = _abstractPanel[itemIndex];
    //        if (AbstractItem.Section > 0)
    //        {
    //            var ret = _abstractPanel.
    //                Where(x => x.Section == AbstractItem.Section - 1).
    //                OrderBy(x => Math.Abs(x.SectionIndex - AbstractItem.SectionIndex)).
    //                First();
    //            return ret._index;
    //        }
    //        else
    //            return itemIndex;
    //    }

    //    private void NavigateDown()
    //    {
    //        var Gen = _generator.GetItemContainerGeneratorForPanel(this);
    //        UIElement Selected = (UIElement)Keyboard.FocusedElement;
    //        int ItemIndex = Gen.IndexFromContainer(Selected);
    //        int Depth = 0;
    //        while (ItemIndex == -1)
    //        {
    //            Selected = (UIElement)VisualTreeHelper.GetParent(Selected);
    //            ItemIndex = Gen.IndexFromContainer(Selected);
    //            Depth++;
    //        }
    //        DependencyObject Next = null;
    //        if (Orientation == Orientation.Horizontal)
    //        {
    //            int NextIndex = GetNextSectionClosestIndex(ItemIndex);
    //            Next = Gen.ContainerFromIndex(NextIndex);
    //            while (Next == null)
    //            {
    //                SetVerticalOffset(VerticalOffset + 1);
    //                UpdateLayout();
    //                Next = Gen.ContainerFromIndex(NextIndex);
    //            }
    //        }
    //        else
    //        {
    //            if (ItemIndex == _abstractPanel._itemCount - 1)
    //                return;
    //            Next = Gen.ContainerFromIndex(ItemIndex + 1);
    //            while (Next == null)
    //            {
    //                SetHorizontalOffset(HorizontalOffset + 1);
    //                UpdateLayout();
    //                Next = Gen.ContainerFromIndex(ItemIndex + 1);
    //            }
    //        }
    //        while (Depth != 0)
    //        {
    //            Next = VisualTreeHelper.GetChild(Next, 0);
    //            Depth--;
    //        }
    //        var UiElement = Next as UIElement;
    //        if (UiElement != null) UiElement.Focus();
    //    }

    //    private void NavigateLeft()
    //    {
    //        var gen = _generator.GetItemContainerGeneratorForPanel(this);

    //        UIElement selected = (UIElement)Keyboard.FocusedElement;
    //        int itemIndex = gen.IndexFromContainer(selected);
    //        int depth = 0;
    //        while (itemIndex == -1)
    //        {
    //            selected = (UIElement)VisualTreeHelper.GetParent(selected);
    //            itemIndex = gen.IndexFromContainer(selected);
    //            depth++;
    //        }
    //        DependencyObject next = null;
    //        if (Orientation == Orientation.Vertical)
    //        {
    //            int nextIndex = GetLastSectionClosestIndex(itemIndex);
    //            next = gen.ContainerFromIndex(nextIndex);
    //            while (next == null)
    //            {
    //                SetHorizontalOffset(HorizontalOffset - 1);
    //                UpdateLayout();
    //                next = gen.ContainerFromIndex(nextIndex);
    //            }
    //        }
    //        else
    //        {
    //            if (itemIndex == 0)
    //                return;
    //            next = gen.ContainerFromIndex(itemIndex - 1);
    //            while (next == null)
    //            {
    //                SetVerticalOffset(VerticalOffset - 1);
    //                UpdateLayout();
    //                next = gen.ContainerFromIndex(itemIndex - 1);
    //            }
    //        }
    //        while (depth != 0)
    //        {
    //            next = VisualTreeHelper.GetChild(next, 0);
    //            depth--;
    //        }
    //        (next as UIElement).Focus();
    //    }

    //    private void NavigateRight()
    //    {
    //        var gen = _generator.GetItemContainerGeneratorForPanel(this);
    //        UIElement selected = (UIElement)Keyboard.FocusedElement;
    //        int itemIndex = gen.IndexFromContainer(selected);
    //        int depth = 0;
    //        while (itemIndex == -1)
    //        {
    //            selected = (UIElement)VisualTreeHelper.GetParent(selected);
    //            itemIndex = gen.IndexFromContainer(selected);
    //            depth++;
    //        }
    //        DependencyObject next = null;
    //        if (Orientation == Orientation.Vertical)
    //        {
    //            int nextIndex = GetNextSectionClosestIndex(itemIndex);
    //            next = gen.ContainerFromIndex(nextIndex);
    //            while (next == null)
    //            {
    //                SetHorizontalOffset(HorizontalOffset + 1);
    //                UpdateLayout();
    //                next = gen.ContainerFromIndex(nextIndex);
    //            }
    //        }
    //        else
    //        {
    //            if (itemIndex == _abstractPanel._itemCount - 1)
    //                return;
    //            next = gen.ContainerFromIndex(itemIndex + 1);
    //            while (next == null)
    //            {
    //                SetVerticalOffset(VerticalOffset + 1);
    //                UpdateLayout();
    //                next = gen.ContainerFromIndex(itemIndex + 1);
    //            }
    //        }
    //        while (depth != 0)
    //        {
    //            next = VisualTreeHelper.GetChild(next, 0);
    //            depth--;
    //        }
    //        (next as UIElement).Focus();
    //    }

    //    private void NavigateUp()
    //    {
    //        var gen = _generator.GetItemContainerGeneratorForPanel(this);
    //        UIElement selected = (UIElement)Keyboard.FocusedElement;
    //        int itemIndex = gen.IndexFromContainer(selected);
    //        int depth = 0;
    //        while (itemIndex == -1)
    //        {
    //            selected = (UIElement)VisualTreeHelper.GetParent(selected);
    //            itemIndex = gen.IndexFromContainer(selected);
    //            depth++;
    //        }
    //        DependencyObject next = null;
    //        if (Orientation == Orientation.Horizontal)
    //        {
    //            int nextIndex = GetLastSectionClosestIndex(itemIndex);
    //            next = gen.ContainerFromIndex(nextIndex);
    //            while (next == null)
    //            {
    //                SetVerticalOffset(VerticalOffset - 1);
    //                UpdateLayout();
    //                next = gen.ContainerFromIndex(nextIndex);
    //            }
    //        }
    //        else
    //        {
    //            if (itemIndex == 0)
    //                return;
    //            next = gen.ContainerFromIndex(itemIndex - 1);
    //            while (next == null)
    //            {
    //                SetHorizontalOffset(HorizontalOffset - 1);
    //                UpdateLayout();
    //                next = gen.ContainerFromIndex(itemIndex - 1);
    //            }
    //        }
    //        while (depth != 0)
    //        {
    //            next = VisualTreeHelper.GetChild(next, 0);
    //            depth--;
    //        }
    //        (next as UIElement).Focus();
    //    }


    //    #endregion

    //    #region Override

    //    protected override void OnKeyDown(KeyEventArgs e)
    //    {
    //        switch (e.Key)
    //        {
    //            case Key.Down:
    //                NavigateDown();
    //                e.Handled = true;
    //                break;
    //            case Key.Left:
    //                NavigateLeft();
    //                e.Handled = true;
    //                break;
    //            case Key.Right:
    //                NavigateRight();
    //                e.Handled = true;
    //                break;
    //            case Key.Up:
    //                NavigateUp();
    //                e.Handled = true;
    //                break;
    //            default:
    //                base.OnKeyDown(e);
    //                break;
    //        }
    //    }


    //    protected override void OnItemsChanged(object sender, ItemsChangedEventArgs args)
    //    {
    //        base.OnItemsChanged(sender, args);
    //        _abstractPanel = null;
    //        ResetScrollInfo();
    //    }

    //    protected override void OnInitialized(EventArgs e)
    //    {
    //        this.SizeChanged += new SizeChangedEventHandler(this.Resizing);
    //        base.OnInitialized(e);
    //        _itemsControl = ItemsControl.GetItemsOwner(this);
    //        _children = InternalChildren;
    //        _generator = ItemContainerGenerator;
    //    }

    //    protected override Size MeasureOverride(Size availableSize)
    //    {
    //        if (_itemsControl == null || _itemsControl.Items.Count == 0)
    //            return availableSize;
    //        if (_abstractPanel == null)
    //            _abstractPanel = new WrapPanelAbstraction(_itemsControl.Items.Count);

    //        _pixelMeasuredViewport = availableSize;

    //        _realizedChildLayout.Clear();

    //        Size RealizedFrameSize = availableSize;

    //        int ItemCount = _itemsControl.Items.Count;
    //        int FirstVisibleIndex = GetFirstVisibleIndex();

    //        GeneratorPosition StartPos = _generator.GeneratorPositionFromIndex(FirstVisibleIndex);

    //        int ChildIndex = (StartPos.Offset == 0) ? StartPos.Index : StartPos.Index + 1;
    //        int Current = FirstVisibleIndex;
    //        int VisibleSections = 1;
    //        using (_generator.StartAt(StartPos, GeneratorDirection.Forward, true))
    //        {
    //            bool stop = false;
    //            bool IsHorizontal = Orientation == Orientation.Horizontal;
    //            double CurrentX = 0;
    //            double CurrentY = 0;
    //            double MaxItemSize = 0;
    //            int CurrentSection = GetFirstVisibleSection();
    //            while (Current < ItemCount)
    //            {
    //                bool NewlyRealized;

    //                // Get or create the child                    
    //                UIElement Child = _generator.GenerateNext(out NewlyRealized) as UIElement;
    //                if (NewlyRealized)
    //                {
    //                    // Figure out if we need to insert the child at the end or somewhere in the middle
    //                    if (ChildIndex >= _children.Count)
    //                    {
    //                        base.AddInternalChild(Child);
    //                    }
    //                    else
    //                    {
    //                        base.InsertInternalChild(ChildIndex, Child);
    //                    }
    //                    _generator.PrepareItemContainer(Child);
    //                    Child.Measure(ChildSlotSize);
    //                }
    //                else
    //                {
    //                    // The child has already been created, let's be sure it's in the right spot
    //                    Debug.Assert(Child == _children[ChildIndex], "Wrong child was generated");
    //                }
    //                _childSize = Child.DesiredSize;
    //                Rect childRect = new Rect(new Point(CurrentX, CurrentY), _childSize);
    //                if (IsHorizontal)
    //                {
    //                    MaxItemSize = Math.Max(MaxItemSize, childRect.Height);
    //                    if (childRect.Right > RealizedFrameSize.Width) //wrap to a new line
    //                    {
    //                        CurrentY = CurrentY + MaxItemSize;
    //                        CurrentX = 0;
    //                        MaxItemSize = childRect.Height;
    //                        childRect.X = CurrentX;
    //                        childRect.Y = CurrentY;
    //                        CurrentSection++;
    //                        VisibleSections++;
    //                    }
    //                    if (CurrentY > RealizedFrameSize.Height)
    //                        stop = true;
    //                    CurrentX = childRect.Right;
    //                }
    //                else
    //                {
    //                    MaxItemSize = Math.Max(MaxItemSize, childRect.Width);
    //                    if (childRect.Bottom > RealizedFrameSize.Height) //wrap to a new column
    //                    {
    //                        CurrentX = CurrentX + MaxItemSize;
    //                        CurrentY = 0;
    //                        MaxItemSize = childRect.Width;
    //                        childRect.X = CurrentX;
    //                        childRect.Y = CurrentY;
    //                        CurrentSection++;
    //                        VisibleSections++;
    //                    }
    //                    if (CurrentX > RealizedFrameSize.Width)
    //                        stop = true;
    //                    CurrentY = childRect.Bottom;
    //                }
    //                _realizedChildLayout.Add(Child, childRect);
    //                _abstractPanel.SetItemSection(Current, CurrentSection);

    //                if (stop)
    //                    break;
    //                Current++;
    //                ChildIndex++;
    //            }
    //        }
    //        CleanUpItems(FirstVisibleIndex, Current - 1);

    //        ComputeExtentAndViewport(availableSize, VisibleSections);

    //        return availableSize;
    //    }
    //    protected override Size ArrangeOverride(Size finalSize)
    //    {
    //        if (_children != null)
    //        {
    //            foreach (UIElement child in _children)
    //            {
    //                var layoutInfo = _realizedChildLayout[child];
    //                child.Arrange(layoutInfo);
    //            }
    //        }
    //        return finalSize;
    //    }

    //    #endregion

    //    #region IScrollInfo Members

    //    private bool _canHScroll = false;
    //    public bool CanHorizontallyScroll
    //    {
    //        get { return _canHScroll; }
    //        set { _canHScroll = value; }
    //    }

    //    private bool _canVScroll = false;
    //    public bool CanVerticallyScroll
    //    {
    //        get { return _canVScroll; }
    //        set { _canVScroll = value; }
    //    }

    //    public double ExtentHeight
    //    {
    //        get { return _extent.Height; }
    //    }

    //    public double ExtentWidth
    //    {
    //        get { return _extent.Width; }
    //    }

    //    public double HorizontalOffset
    //    {
    //        get { return _offset.X; }
    //    }

    //    public double VerticalOffset
    //    {
    //        get { return _offset.Y; }
    //    }

    //    public void LineDown()
    //    {
    //        if (Orientation == Orientation.Vertical)
    //            SetVerticalOffset(VerticalOffset + 20);
    //        else
    //            SetVerticalOffset(VerticalOffset + 1);
    //    }

    //    public void LineLeft()
    //    {
    //        if (Orientation == Orientation.Horizontal)
    //            SetHorizontalOffset(HorizontalOffset - 20);
    //        else
    //            SetHorizontalOffset(HorizontalOffset - 1);
    //    }

    //    public void LineRight()
    //    {
    //        if (Orientation == Orientation.Horizontal)
    //            SetHorizontalOffset(HorizontalOffset + 20);
    //        else
    //            SetHorizontalOffset(HorizontalOffset + 1);
    //    }

    //    public void LineUp()
    //    {
    //        if (Orientation == Orientation.Vertical)
    //            SetVerticalOffset(VerticalOffset - 20);
    //        else
    //            SetVerticalOffset(VerticalOffset - 1);
    //    }

    //    public Rect MakeVisible(Visual visual, Rect rectangle)
    //    {
    //        var gen = (ItemContainerGenerator)_generator.GetItemContainerGeneratorForPanel(this);
    //        var element = (UIElement)visual;
    //        int itemIndex = gen.IndexFromContainer(element);
    //        while (itemIndex == -1)
    //        {
    //            element = (UIElement)VisualTreeHelper.GetParent(element);
    //            itemIndex = gen.IndexFromContainer(element);
    //        }
    //        int section = _abstractPanel[itemIndex].Section;
    //        Rect elementRect = _realizedChildLayout[element];
    //        if (Orientation == Orientation.Horizontal)
    //        {
    //            double viewportHeight = _pixelMeasuredViewport.Height;
    //            if (elementRect.Bottom > viewportHeight)
    //                _offset.Y += 1;
    //            else if (elementRect.Top < 0)
    //                _offset.Y -= 1;
    //        }
    //        else
    //        {
    //            double viewportWidth = _pixelMeasuredViewport.Width;
    //            if (elementRect.Right > viewportWidth)
    //                _offset.X += 1;
    //            else if (elementRect.Left < 0)
    //                _offset.X -= 1;
    //        }
    //        InvalidateMeasure();
    //        return elementRect;
    //    }

    //    public void MouseWheelDown()
    //    {
    //        PageDown();
    //    }

    //    public void MouseWheelLeft()
    //    {
    //        PageLeft();
    //    }

    //    public void MouseWheelRight()
    //    {
    //        PageRight();
    //    }

    //    public void MouseWheelUp()
    //    {
    //        PageUp();
    //    }

    //    public void PageDown()
    //    {
    //        SetVerticalOffset(VerticalOffset + _viewport.Height * 0.8);
    //    }

    //    public void PageLeft()
    //    {
    //        SetHorizontalOffset(HorizontalOffset - _viewport.Width * 0.8);
    //    }

    //    public void PageRight()
    //    {
    //        SetHorizontalOffset(HorizontalOffset + _viewport.Width * 0.8);
    //    }

    //    public void PageUp()
    //    {
    //        SetVerticalOffset(VerticalOffset - _viewport.Height * 0.8);
    //    }

    //    private ScrollViewer _owner;
    //    public ScrollViewer ScrollOwner
    //    {
    //        get { return _owner; }
    //        set { _owner = value; }
    //    }

    //    public void SetHorizontalOffset(double offset)
    //    {
    //        if (offset < 0 || _viewport.Width >= _extent.Width)
    //        {
    //            offset = 0;
    //        }
    //        else
    //        {
    //            if (offset + _viewport.Width >= _extent.Width)
    //            {
    //                offset = _extent.Width - _viewport.Width;
    //            }
    //        }

    //        _offset.X = offset;

    //        if (_owner != null)
    //            _owner.InvalidateScrollInfo();

    //        InvalidateMeasure();
    //        _firstIndex = GetFirstVisibleIndex();
    //    }

    //    public void SetVerticalOffset(double offset)
    //    {
    //        if (offset < 0 || _viewport.Height >= _extent.Height)
    //        {
    //            offset = 0;
    //        }
    //        else
    //        {
    //            if (offset + _viewport.Height >= _extent.Height)
    //            {
    //                offset = _extent.Height - _viewport.Height;
    //            }
    //        }

    //        _offset.Y = offset;

    //        if (_owner != null)
    //            _owner.InvalidateScrollInfo();

    //        //_trans.Y = -offset;

    //        InvalidateMeasure();
    //        _firstIndex = GetFirstVisibleIndex();
    //    }

    //    public double ViewportHeight
    //    {
    //        get { return _viewport.Height; }
    //    }

    //    public double ViewportWidth
    //    {
    //        get { return _viewport.Width; }
    //    }

    //    #endregion

    //    #region helper data structures

    //    class ItemAbstraction
    //    {
    //        public ItemAbstraction(WrapPanelAbstraction panel, int index)
    //        {
    //            _panel = panel;
    //            _index = index;
    //        }

    //        WrapPanelAbstraction _panel;

    //        public readonly int _index;

    //        int _sectionIndex = -1;
    //        public int SectionIndex
    //        {
    //            get
    //            {
    //                if (_sectionIndex == -1)
    //                {
    //                    return _index % _panel._averageItemsPerSection - 1;
    //                }
    //                return _sectionIndex;
    //            }
    //            set
    //            {
    //                if (_sectionIndex == -1)
    //                    _sectionIndex = value;
    //            }
    //        }

    //        int _section = -1;
    //        public int Section
    //        {
    //            get
    //            {
    //                if (_section == -1)
    //                {
    //                    return _index / _panel._averageItemsPerSection;
    //                }
    //                return _section;
    //            }
    //            set
    //            {
    //                if (_section == -1)
    //                    _section = value;
    //            }
    //        }
    //    }

    //    class WrapPanelAbstraction : IEnumerable<ItemAbstraction>
    //    {
    //        public WrapPanelAbstraction(int itemCount)
    //        {
    //            List<ItemAbstraction> items = new List<ItemAbstraction>(itemCount);
    //            for (int i = 0; i < itemCount; i++)
    //            {
    //                ItemAbstraction item = new ItemAbstraction(this, i);
    //                items.Add(item);
    //            }

    //            Items = new ReadOnlyCollection<ItemAbstraction>(items);
    //            _averageItemsPerSection = itemCount;
    //            _itemCount = itemCount;
    //        }

    //        public readonly int _itemCount;
    //        public int _averageItemsPerSection;
    //        private int _currentSetSection = -1;
    //        private int _currentSetItemIndex = -1;
    //        private int _itemsInCurrentSecction = 0;
    //        private object _syncRoot = new object();

    //        public int SectionCount
    //        {
    //            get
    //            {
    //                int ret = _currentSetSection + 1;
    //                if (_currentSetItemIndex + 1 < Items.Count)
    //                {
    //                    int itemsLeft = Items.Count - _currentSetItemIndex;
    //                    ret += itemsLeft / _averageItemsPerSection + 1;
    //                }
    //                return ret;
    //            }
    //        }

    //        private ReadOnlyCollection<ItemAbstraction> Items { get; set; }

    //        public void SetItemSection(int index, int section)
    //        {
    //            lock (_syncRoot)
    //            {
    //                if (section <= _currentSetSection + 1 && index == _currentSetItemIndex + 1)
    //                {
    //                    _currentSetItemIndex++;
    //                    Items[index].Section = section;
    //                    if (section == _currentSetSection + 1)
    //                    {
    //                        _currentSetSection = section;
    //                        if (section > 0)
    //                        {
    //                            _averageItemsPerSection = (index) / (section);
    //                        }
    //                        _itemsInCurrentSecction = 1;
    //                    }
    //                    else
    //                        _itemsInCurrentSecction++;
    //                    Items[index].SectionIndex = _itemsInCurrentSecction - 1;
    //                }
    //            }
    //        }

    //        public ItemAbstraction this[int index]
    //        {
    //            get { return Items[index]; }
    //        }

    //        #region IEnumerable<ItemAbstraction> Members

    //        public IEnumerator<ItemAbstraction> GetEnumerator()
    //        {
    //            return Items.GetEnumerator();
    //        }

    //        #endregion

    //        #region IEnumerable Members

    //        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    //        {
    //            return GetEnumerator();
    //        }

    //        #endregion
    //    }

    //    #endregion
    //}

    //#endregion

    #region version 2
    public class VirtualizingWrapPanel : VirtualizingPanel, IScrollInfo
    {
        private const double ScrollLineAmount = 16.0;

        private Size _extentSize;
        private Size _viewportSize;
        private Point _offset;
        private ItemsControl _itemsControl;
        private readonly Dictionary<UIElement, Rect> _childLayouts = new Dictionary<UIElement, Rect>();

        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register("ItemWidth", typeof(double), typeof(VirtualizingWrapPanel), new PropertyMetadata(1.0, HandleItemDimensionChanged));

        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register("ItemHeight", typeof(double), typeof(VirtualizingWrapPanel), new PropertyMetadata(1.0, HandleItemDimensionChanged));

        private static readonly DependencyProperty VirtualItemIndexProperty =
            DependencyProperty.RegisterAttached("VirtualItemIndex", typeof(int), typeof(VirtualizingWrapPanel), new PropertyMetadata(-1));
        private IRecyclingItemContainerGenerator _itemsGenerator;

        private bool _isInMeasure;

        private static int GetVirtualItemIndex(DependencyObject obj)
        {
            return (int)obj.GetValue(VirtualItemIndexProperty);
        }

        private static void SetVirtualItemIndex(DependencyObject obj, int value)
        {
            obj.SetValue(VirtualItemIndexProperty, value);
        }

        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        public VirtualizingWrapPanel()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                Dispatcher.BeginInvoke((Action)Initialize);
            }
        }

        private void Initialize()
        {
            _itemsControl = ItemsControl.GetItemsOwner(this);
            _itemsGenerator = (IRecyclingItemContainerGenerator)ItemContainerGenerator;

            InvalidateMeasure();
        }

        protected override void OnItemsChanged(object sender, ItemsChangedEventArgs args)
        {
            base.OnItemsChanged(sender, args);

            InvalidateMeasure();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (_itemsControl == null)
            {
                return availableSize;
            }

            _isInMeasure = true;
            _childLayouts.Clear();

            var extentInfo = GetExtentInfo(availableSize, ItemHeight);

            EnsureScrollOffsetIsWithinConstrains(extentInfo);

            var layoutInfo = GetLayoutInfo(availableSize, ItemHeight, extentInfo);

            RecycleItems(layoutInfo);

            // Determine where the first item is in relation to previously realized items
            var generatorStartPosition = _itemsGenerator.GeneratorPositionFromIndex(layoutInfo.FirstRealizedItemIndex);

            var visualIndex = 0;

            var currentX = layoutInfo.FirstRealizedItemLeft;
            var currentY = layoutInfo.FirstRealizedLineTop;

            using (_itemsGenerator.StartAt(generatorStartPosition, GeneratorDirection.Forward, true))
            {
                for (var itemIndex = layoutInfo.FirstRealizedItemIndex; itemIndex <= layoutInfo.LastRealizedItemIndex; itemIndex++, visualIndex++)
                {
                    bool newlyRealized;

                    var child = (UIElement)_itemsGenerator.GenerateNext(out newlyRealized);
                    SetVirtualItemIndex(child, itemIndex);

                    if (newlyRealized)
                    {
                        InsertInternalChild(visualIndex, child);
                    }
                    else
                    {
                        // check if item needs to be moved into a new position in the Children collection
                        if (visualIndex < Children.Count)
                        {
                            if (Children[visualIndex] != child)
                            {
                                var childCurrentIndex = Children.IndexOf(child);

                                if (childCurrentIndex >= 0)
                                {
                                    RemoveInternalChildRange(childCurrentIndex, 1);
                                }

                                InsertInternalChild(visualIndex, child);
                            }
                        }
                        else
                        {
                            // we know that the child can't already be in the children collection
                            // because we've been inserting children in correct visualIndex order,
                            // and this child has a visualIndex greater than the Children.Count
                            AddInternalChild(child);
                        }
                    }

                    // only prepare the item once it has been added to the visual tree
                    _itemsGenerator.PrepareItemContainer(child);

                    child.Measure(new Size(ItemWidth, ItemHeight));

                    _childLayouts.Add(child, new Rect(currentX, currentY, ItemWidth, ItemHeight));

                    if (currentX + ItemWidth * 2 >= availableSize.Width)
                    {
                        // wrap to a new line
                        currentY += ItemHeight;
                        currentX = 0;
                    }
                    else
                    {
                        currentX += ItemWidth;
                    }
                }
            }

            RemoveRedundantChildren();
            UpdateScrollInfo(availableSize, extentInfo);

            var desiredSize = new Size(double.IsInfinity(availableSize.Width) ? 0 : availableSize.Width,
                                       double.IsInfinity(availableSize.Height) ? 0 : availableSize.Height);

            _isInMeasure = false;

            return desiredSize;
        }

        private void EnsureScrollOffsetIsWithinConstrains(ExtentInfo extentInfo)
        {
            _offset.Y = Clamp(_offset.Y, 0, extentInfo.MaxVerticalOffset);
        }

        private void RecycleItems(ItemLayoutInfo layoutInfo)
        {
            foreach (UIElement child in Children)
            {
                var virtualItemIndex = GetVirtualItemIndex(child);

                if (virtualItemIndex < layoutInfo.FirstRealizedItemIndex || virtualItemIndex > layoutInfo.LastRealizedItemIndex)
                {
                    var generatorPosition = _itemsGenerator.GeneratorPositionFromIndex(virtualItemIndex);
                    if (generatorPosition.Index >= 0)
                    {
                        _itemsGenerator.Recycle(generatorPosition, 1);
                    }
                }

                SetVirtualItemIndex(child, -1);
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement child in Children)
            {
                child.Arrange(_childLayouts[child]);
            }

            return finalSize;
        }

        private void UpdateScrollInfo(Size availableSize, ExtentInfo extentInfo)
        {
            _viewportSize = availableSize;
            _extentSize = new Size(availableSize.Width, extentInfo.ExtentHeight);

            InvalidateScrollInfo();
        }

        private void RemoveRedundantChildren()
        {
            // iterate backwards through the child collection because we're going to be
            // removing items from it
            for (var i = Children.Count - 1; i >= 0; i--)
            {
                var child = Children[i];

                // if the virtual item index is -1, this indicates
                // it is a recycled item that hasn't been reused this time round
                if (GetVirtualItemIndex(child) == -1)
                {
                    RemoveInternalChildRange(i, 1);
                }
            }
        }

        private ItemLayoutInfo GetLayoutInfo(Size availableSize, double itemHeight, ExtentInfo extentInfo)
        {
            if (_itemsControl == null)
            {
                return new ItemLayoutInfo();
            }

            // we need to ensure that there is one realized item prior to the first visible item, and one after the last visible item,
            // so that keyboard navigation works properly. For example, when focus is on the first visible item, and the user
            // navigates up, the ListBox selects the previous item, and the scrolls that into view - and this triggers the loading of the rest of the items 
            // in that row

            var firstVisibleLine = (int)Math.Floor(VerticalOffset / itemHeight);

            var firstRealizedIndex = Math.Max(extentInfo.ItemsPerLine * firstVisibleLine - 1, 0);
            var firstRealizedItemLeft = firstRealizedIndex % extentInfo.ItemsPerLine * ItemWidth - HorizontalOffset;
            var firstRealizedItemTop = (firstRealizedIndex / extentInfo.ItemsPerLine) * itemHeight - VerticalOffset;

            var firstCompleteLineTop = (firstVisibleLine == 0 ? firstRealizedItemTop : firstRealizedItemTop + ItemHeight);
            var completeRealizedLines = (int)Math.Ceiling((availableSize.Height - firstCompleteLineTop) / itemHeight);

            var lastRealizedIndex = Math.Min(firstRealizedIndex + completeRealizedLines * extentInfo.ItemsPerLine + 2, _itemsControl.Items.Count - 1);

            return new ItemLayoutInfo
            {
                FirstRealizedItemIndex = firstRealizedIndex,
                FirstRealizedItemLeft = firstRealizedItemLeft,
                FirstRealizedLineTop = firstRealizedItemTop,
                LastRealizedItemIndex = lastRealizedIndex,
            };
        }

        private ExtentInfo GetExtentInfo(Size viewPortSize, double itemHeight)
        {
            if (_itemsControl == null)
            {
                return new ExtentInfo();
            }

            var itemsPerLine = Math.Max((int)Math.Floor(viewPortSize.Width / ItemWidth), 1);
            var totalLines = (int)Math.Ceiling((double)_itemsControl.Items.Count / itemsPerLine);
            var extentHeight = Math.Max(totalLines * ItemHeight, viewPortSize.Height);

            return new ExtentInfo
            {
                ItemsPerLine = itemsPerLine,
                TotalLines = totalLines,
                ExtentHeight = extentHeight,
                MaxVerticalOffset = extentHeight - viewPortSize.Height,
            };
        }

        public void LineUp()
        {
            SetVerticalOffset(VerticalOffset - ScrollLineAmount);
        }

        public void LineDown()
        {
            SetVerticalOffset(VerticalOffset + ScrollLineAmount);
        }

        public void LineLeft()
        {
            SetHorizontalOffset(HorizontalOffset + ScrollLineAmount);
        }

        public void LineRight()
        {
            SetHorizontalOffset(HorizontalOffset - ScrollLineAmount);
        }

        public void PageUp()
        {
            SetVerticalOffset(VerticalOffset - ViewportHeight);
        }

        public void PageDown()
        {
            SetVerticalOffset(VerticalOffset + ViewportHeight);
        }

        public void PageLeft()
        {
            SetHorizontalOffset(HorizontalOffset + ItemWidth);
        }

        public void PageRight()
        {
            SetHorizontalOffset(HorizontalOffset - ItemWidth);
        }

        public void MouseWheelUp()
        {
            SetVerticalOffset(VerticalOffset - ScrollLineAmount * SystemParameters.WheelScrollLines);
        }

        public void MouseWheelDown()
        {
            SetVerticalOffset(VerticalOffset + ScrollLineAmount * SystemParameters.WheelScrollLines);
        }

        public void MouseWheelLeft()
        {
            SetHorizontalOffset(HorizontalOffset - ScrollLineAmount * SystemParameters.WheelScrollLines);
        }

        public void MouseWheelRight()
        {
            SetHorizontalOffset(HorizontalOffset + ScrollLineAmount * SystemParameters.WheelScrollLines);
        }

        public void SetHorizontalOffset(double offset)
        {
            if (_isInMeasure)
            {
                return;
            }

            offset = Clamp(offset, 0, ExtentWidth - ViewportWidth);
            _offset = new Point(offset, _offset.Y);

            InvalidateScrollInfo();
            InvalidateMeasure();
        }

        public void SetVerticalOffset(double offset)
        {
            if (_isInMeasure)
            {
                return;
            }

            offset = Clamp(offset, 0, ExtentHeight - ViewportHeight);
            _offset = new Point(_offset.X, offset);

            InvalidateScrollInfo();
            InvalidateMeasure();
        }

        public Rect MakeVisible(Visual visual, Rect rectangle)
        {
            if (rectangle.IsEmpty ||
                visual == null ||
                visual == this ||
                !IsAncestorOf(visual))
            {
                return Rect.Empty;
            }

            rectangle = visual.TransformToAncestor(this).TransformBounds(rectangle);

            var viewRect = new Rect(HorizontalOffset, VerticalOffset, ViewportWidth, ViewportHeight);
            rectangle.X += viewRect.X;
            rectangle.Y += viewRect.Y;

            viewRect.X = CalculateNewScrollOffset(viewRect.Left, viewRect.Right, rectangle.Left, rectangle.Right);
            viewRect.Y = CalculateNewScrollOffset(viewRect.Top, viewRect.Bottom, rectangle.Top, rectangle.Bottom);

            SetHorizontalOffset(viewRect.X);
            SetVerticalOffset(viewRect.Y);
            rectangle.Intersect(viewRect);

            rectangle.X -= viewRect.X;
            rectangle.Y -= viewRect.Y;

            return rectangle;
        }

        private static double CalculateNewScrollOffset(double topView, double bottomView, double topChild, double bottomChild)
        {
            var offBottom = topChild < topView && bottomChild < bottomView;
            var offTop = bottomChild > bottomView && topChild > topView;
            var tooLarge = (bottomChild - topChild) > (bottomView - topView);

            if (!offBottom && !offTop)
                return topView;

            if ((offBottom && !tooLarge) || (offTop && tooLarge))
                return topChild;

            return bottomChild - (bottomView - topView);
        }


        public ItemLayoutInfo GetVisibleItemsRange()
        {
            return GetLayoutInfo(_viewportSize, ItemHeight, GetExtentInfo(_viewportSize, ItemHeight));
        }

        public bool CanVerticallyScroll
        {
            get;
            set;
        }

        public bool CanHorizontallyScroll
        {
            get;
            set;
        }

        public double ExtentWidth
        {
            get { return _extentSize.Width; }
        }

        public double ExtentHeight
        {
            get { return _extentSize.Height; }
        }

        public double ViewportWidth
        {
            get { return _viewportSize.Width; }
        }

        public double ViewportHeight
        {
            get { return _viewportSize.Height; }
        }

        public double HorizontalOffset
        {
            get { return _offset.X; }
        }

        public double VerticalOffset
        {
            get { return _offset.Y; }
        }

        public ScrollViewer ScrollOwner
        {
            get;
            set;
        }

        private void InvalidateScrollInfo()
        {
            if (ScrollOwner != null)
            {
                ScrollOwner.InvalidateScrollInfo();
            }
        }

        private static void HandleItemDimensionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var wrapPanel = (d as VirtualizingWrapPanel);

            if (wrapPanel != null)
                wrapPanel.InvalidateMeasure();
        }

        private double Clamp(double value, double min, double max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        internal class ExtentInfo
        {
            public int ItemsPerLine;
            public int TotalLines;
            public double ExtentHeight;
            public double MaxVerticalOffset;
        }

        public class ItemLayoutInfo
        {
            public int FirstRealizedItemIndex;
            public double FirstRealizedLineTop;
            public double FirstRealizedItemLeft;
            public int LastRealizedItemIndex;
        }
    }

    #endregion
}
