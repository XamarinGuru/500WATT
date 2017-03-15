﻿
using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using PortableLibrary;

namespace goheja
{
	[Activity(Label = "AddCommentActivity", ScreenOrientation = ScreenOrientation.Portrait)]
	public class AddCommentActivity : BaseActivity
	{
		private RootMemberModel MemberModel = new RootMemberModel();

		EditText txtComment;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.AddCommentActivity);

			if (!IsNetEnable()) return;

			System.Threading.ThreadPool.QueueUserWorkItem(delegate
			{
				ShowLoadingView(Constants.MSG_LOADING_DATA);

				MemberModel.rootMember = GetUserObject();

				HideLoadingView();
			});

			InitUISettings();
		}

		void InitUISettings()
		{
			txtComment = FindViewById<EditText>(Resource.Id.txtComment);
			FindViewById(Resource.Id.ActionAddComment).Click += ActionAddComment;
		}

		void ActionAddComment(object sender, EventArgs e)
		{
			if (txtComment.Text == "")
			{
				ShowMessageBox(null, Constants.MSG_TYPE_COMMENT);
				return;
			}

			if (!IsNetEnable()) return;

			var author = MemberModel.firstname + " " + MemberModel.lastname;
			var authorID = AppSettings.UserID;

			System.Threading.ThreadPool.QueueUserWorkItem(delegate
			{
				ShowLoadingView(Constants.MSG_SAVE_COMMENT);

				var response = SetComment(author, authorID, txtComment.Text, AppSettings.selectedEvent._id);

				HideLoadingView();

				var activity = new Intent();
				SetResult(Result.Canceled, activity);
				Finish();
			});
		}

		public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
		{
			if (keyCode == Keycode.Back)
			{
				var activity = new Intent();
				SetResult(Result.Canceled, activity);
				Finish();
			}

			return base.OnKeyDown(keyCode, e);
		}
	}
}
