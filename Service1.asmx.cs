using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Web;
using System.Web.Services;
using Oracle.DataAccess.Client;

namespace is2maintenance
{
	/// <summary>
	/// [is2maintenance]
	/// </summary>
	//-------------------------------------------------------------------------------------
	// 修正履歴
	//-------------------------------------------------------------------------------------
	// ADD 2007.04.28 東都）高木 オブジェクトの破棄
	//	disposeReader(reader);
	//	reader = null;
	// DEL 2007.05.10 東都）高木 未使用関数のコメント化
	//	logFileOpen(sUser);
	//	userCheck2(conn2, sUser);
	//	logFileClose();
	// MOD 2007.08.29 東都）高木 稼働率集計の変更
	//	福山通運分は別で集計して加算する
	// MOD 2007.10.11 東都）高木 デモ機を除く機能の追加
	//	デモ機を除く機能の追加
	// MOD 2007.10.22 東都）高木 運賃に中継料を加算表示
	// ADD 2007.11.14 KCL) 森本 global対応の会員マスタ一覧取得を追加
	// ADD 2007.11.14 KCL) 森本 global対応の会員名等データ取得を追加
	// ADD 2007.11.14 KCL) 森本 global対応のご依頼主一覧取得を追加
	// ADD 2007.11.22 東都）高木 一覧項目に発店ＣＤを表示
	//-------------------------------------------------------------------------------------
	// ADD 2008.03.21 東都）グローバル対応
	// ADD 2008.02.14 東都）高木 セッション数の取得
	// ADD 2008.05.21 東都）高木 ログインエラー回数を５回にする 
	// ADD 2008.05.29 東都）高木 パスワード更新年月日を表示 
	// MOD 2008.11.26 東都）高木 部課コードが空白でもエラーがでなくする 
	// MOD 2008.12.01 東都）高木 出荷照会の一覧のソート順の訂正 
	// DEL 2008.12.03 東都）高木 荷送人存在チェックから部門ＣＤのしばりをはずす 
	// MOD 2008.12.08 東都）高木 営業所でのパスワード照会対応 
	//-------------------------------------------------------------------------------------
	// ADD 2009.01.06 東都）高木 パスワードチェック対応 
	// ADD 2009.03.03 東都）高木 サーマルシリアル番号の追加 
	// ADD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） 
	// ADD 2009.04.02 東都）高木 稼働日対応 
	// MOD 2009.05.11 東都）高木 未出荷対応 
	// MOD 2009.05.28 東都）高木 出荷実績一覧運賃非表示対応 
	// MOD 2009.06.23 東都）高木 住所マスタのプライマリーキー変更 
	// MOD 2009.07.09 東都）高木 配完情報検索機能の追加 
	// MOD 2009.09.11 東都）高木 出荷照会で出荷済ＦＧ,送信済ＦＧなどを追加 
	// MOD 2009.11.16 東都）高木 集約店を一覧に追加 
	// MOD 2009.11.25 東都）高木 お客様情報出力（ＣＳＶ）の項目追加 
	//-------------------------------------------------------------------------------------
	// MOD 2010.04.06 東都）高木 出荷照会に得意先、郵便番号などを追加 
	// MOD 2010.04.13 東都）高木 郵便番号が削除された時の障害対応 
	// MOD 2010.04.27 東都）高木 運用中のお客様のみ対象機能の追加 
	// MOD 2010.04.30 東都）高木 ＣＳＶ出力機能の追加 
	//保留 MOD 2010.07.21 東都）高木 リコー様対応 
	// MOD 2010.08.26 東都）高木 お客様、セクション、請求先削除時のログ強化 
	//保留 MOD 2010.10.12 東都）高木 運賃エラー対応 
	// MOD 2010.11.19 東都）高木 配完情報などの取得 
	// MOD 2010.11.25 東都）高木 出荷照会に削除日時などを追加 
	// ADD 2010.12.14 ACT）垣原 王子運送の対応 
	//-------------------------------------------------------------------------------------
	// MOD 2011.02.02 東都）高木 出荷データ出荷日範囲取得 
	// MOD 2011.03.09 東都）高木 請求先マスタの主キーに[会員ＣＤ]を追加 
	// MOD 2011.04.13 東都）高木 重量入力不可対応 
	// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 
	// MOD 2011.05.31 東都）高木 王子運送の対応 
	//-------------------------------------------------------------------------------------
	// MOD 2014.03.19 BEVAS）高杉 出荷照会に配完日付・時刻を追加
	// MOD 2014.03.20 BEVAS）高杉 ＣＳＶ出力に配完日付・時刻を追加
	//-------------------------------------------------------------------------------------
	// ADD 2014.09.10 BEVAS)前田　支店止め機能対応
	//-------------------------------------------------------------------------------------
	// MOD 2015.10.09 bevas）松本 発店仕分けコード登録画面に自店所と東京テリトリ店を表示
	//-------------------------------------------------------------------------------------
	// ADD 2015.11.24 bevas）松本 出荷実績表・出荷ラベルイメージ印刷機能追加(is-2管理)
	//-------------------------------------------------------------------------------------
	// MOD 2015.12.15 bevas) 松本 輸送禁止エリア機能対応(is-2管理：ラベルイメージ印刷時)
	//-------------------------------------------------------------------------------------
	// MOD 2016.04.08 bevas) 松本 社内伝票機能追加対応
	//                            ※併せて以下の対応を実施
	//                                ＞ラベルイメージの参照テーブル拡張
	//                                ＞王子運送の稼働率集計を抽出条件に追加（導入台数関連）
	//-------------------------------------------------------------------------------------

	[System.Web.Services.WebService(
		 Namespace="http://Walkthrough/XmlWebServices/",
		 Description="is2maintenance")]

	public class Service1 : is2common.CommService
	{
		private static string sCRLF = "\\r\\n";
		private static string sSepa = "|";
		private static string sKanma = ",";
		private static string sDbl = "\"";
		private static string sSng = "'";

		public Service1()
		{
			//CODEGEN: この呼び出しは、ASP.NET Web サービス デザイナで必要です。
			InitializeComponent();

			connectService();
		}

		#region コンポーネント デザイナで生成されたコード 
		
		//Web サービス デザイナで必要です。
		private IContainer components = null;
				
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

		/*********************************************************************
		 * 会員マスタ取得
		 * 引数：会員ＣＤ
		 * 戻値：ステータス、会員ＣＤ、会員名、使用開始日、管理者区分、使用終了日
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_Member(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "会員マスタ検索開始");

			OracleConnection conn2 = null;
// MOD 2009.05.28 東都）高木 出荷実績一覧運賃非表示対応 START
//			string[] sRet = new string[7];
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 START
//			string[] sRet = new string[8];
			string[] sRet = new string[9];
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 END
// MOD 2009.05.28 東都）高木 出荷実績一覧運賃非表示対応 END

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT 会員ＣＤ "
					+       ",会員名 "
					+       ",使用開始日 "
					+       ",管理者区分 "
					+       ",使用終了日 "
					+       ",更新日時 \n"
// MOD 2009.05.28 東都）高木 出荷実績一覧運賃非表示対応 START
					+       ",記事連携ＦＧ \n"
// MOD 2009.05.28 東都）高木 出荷実績一覧運賃非表示対応 END
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 START
					+       ",保留印刷ＦＧ \n"
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 END
					+  " FROM ＣＭ０１会員 \n"
					+ " WHERE 会員ＣＤ = '" + sKey[0] + "' \n"
					+    "AND 削除ＦＧ = '0' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				while (reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();
					sRet[2] = reader.GetString(1).Trim();
					sRet[3] = reader.GetString(2).Trim();
					sRet[4] = reader.GetString(3).Trim();
					sRet[5] = reader.GetString(4).Trim();
					sRet[6] = reader.GetDecimal(5).ToString().Trim();
// MOD 2009.05.28 東都）高木 出荷実績一覧運賃非表示対応 START
					sRet[7] = reader.GetString(6);
// MOD 2009.05.28 東都）高木 出荷実績一覧運賃非表示対応 END
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 START
					sRet[8] = reader.GetString(7);
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 END
					iCnt++;
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				if(iCnt == 1) 
					sRet[0] = "該当データがありません";
				else
					sRet[0] = "正常終了";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 会員マスタ一覧取得
		 * 引数：会員ＣＤ、会員名
		 * 戻値：ステータス、会員ＣＤ、会員名
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Get_Member(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "会員マスタ一覧取得開始");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' "
					+     "|| TRIM(会員ＣＤ) || '|' "
					+     "|| TRIM(会員名) || '|' \n"
// MOD 2010.04.27 東都）高木 運用中のお客様のみ対象機能の追加 START
					+     "|| TRIM(使用終了日) || '|' "
					+     "|| TO_CHAR(SYSDATE,'YYYYMMDD') || '|' \n"
// MOD 2010.04.27 東都）高木 運用中のお客様のみ対象機能の追加 END
					+  " FROM ＣＭ０１会員 \n";
				if (sKey[0].Trim().Length == 12)
				{
					cmdQuery += " WHERE 会員ＣＤ = '" + sKey[0] + "' \n";
				}
				else
				{
					cmdQuery += " WHERE 会員ＣＤ LIKE '" + sKey[0] + "%' \n";
				}
				if (sKey[1].Trim().Length != 0)
				{
// MOD 2006.06.28 東都）高木 中間一致に修正 START
//					cmdQuery += " AND 会員名 LIKE '" + sKey[1] + "%' \n";
					cmdQuery += " AND 会員名 LIKE '%" + sKey[1] + "%' \n";
// MOD 2006.06.28 東都）高木 中間一致に修正 END
				}
// MOD 2010.04.27 東都）高木 運用中のお客様のみ対象機能の追加 START
				if(sKey.Length >= 4){
					if(sKey[3] == "1"){
						cmdQuery += " AND 使用終了日 >= TO_CHAR(SYSDATE,'YYYYMMDD') \n";
					}
				}
// MOD 2010.04.27 東都）高木 運用中のお客様のみ対象機能の追加 END
				cmdQuery += " AND 削除ＦＧ = '0' \n";
				cmdQuery += " ORDER BY 会員ＣＤ \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);

				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0)
				{
					sRet[0] = "該当データがありません";
				}
				else
				{
					sRet[0] = "正常終了";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 会員マスタ追加
		 * 引数：会員ＣＤ、会員名...
		 * 戻値：ステータス、更新日時
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Ins_Member(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "会員マスタ追加開始");
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 START
			string s保留印刷ＦＧ = (sKey.Length > 7) ? sKey[7] : "0";
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 END

			OracleConnection conn2 = null;
			string[] sRet = new string[2];
			string s更新日時 = System.DateTime.Now.ToString("yyyyMMddHHmmss");

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT 削除ＦＧ "
					+   "FROM ＣＭ０１会員 "
					+  "WHERE 会員ＣＤ = '" + sKey[0] + "' "
					+    "FOR UPDATE \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				string s削除ＦＧ = "";
				while (reader.Read())
				{
					s削除ＦＧ = reader.GetString(0);
					iCnt++;
				}
				if(iCnt == 1)
				{
					//追加
					cmdQuery
						= "INSERT INTO ＣＭ０１会員 \n"
						+ " VALUES ('" + sKey[0] + "' " 
						+         ",'" + sKey[1] + "' "
						+         ",'" + sKey[2] + "' "
						+         ",'" + sKey[3] + "' "
						+         ",'" + sKey[4] + "' "
						+         ",'0' "
						+         ",'0' "
						+         ",'0' "
						+         ",'0' "
						+         ",'0' "
						+         ",'0' "
						+         ",'0' "
						+         ",' ' "
						+         ", 0 "
						+         ", 0 "
						+         ", 0 "
						+         ", 0 "
						+         ", 0 "
						+         ",'0' "
						+         "," + s更新日時
						+         ",'会員登録' "
						+         ",'" + sKey[6] + "' "
						+         "," + s更新日時
						+         ",'会員登録' "
						+         ",'" + sKey[6] + "' "
						+ " ) \n";

					CmdUpdate(sUser, conn2, cmdQuery);

					tran.Commit();
					sRet[0] = "正常終了";
					sRet[1] = s更新日時;
				}
				else
				{
					//追加更新
					if (s削除ＦＧ.Equals("1"))
					{
						cmdQuery
							= "UPDATE ＣＭ０１会員 \n"
							+   " SET 会員名 = '" + sKey[1] + "' "
							+       ",使用開始日 = '" + sKey[2] + "' "
							+       ",使用終了日 = '" + sKey[3] + "' "
							+       ",管理者区分 = '" + sKey[4] + "' "
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 START
							+       ",記事連携ＦＧ = '0' \n"
							+       ",保留印刷ＦＧ = '" + s保留印刷ＦＧ + "' \n"
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 END
							+       ",削除ＦＧ = '0' \n"
							+       ",登録日時 = " + s更新日時
							+       ",登録ＰＧ = '会員登録' "
							+       ",登録者 = '" + sKey[6] + "' "
							+       ",更新日時 = " + s更新日時
							+       ",更新ＰＧ = '会員登録' "
							+       ",更新者 = '" + sKey[6] + "' \n"
							+ " WHERE 会員ＣＤ = '" + sKey[0] + "' \n";

						CmdUpdate(sUser, conn2, cmdQuery);

						tran.Commit();
						sRet[0] = "正常終了";
						sRet[1] = s更新日時;
					}
					else
					{
						tran.Rollback();
						sRet[0] = "既に登録されています";
					}
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 会員マスタ更新
		 * 引数：会員ＣＤ、会員名...
		 * 戻値：ステータス、更新日時
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Upd_Member(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "会員マスタ更新開始");
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 START
			string s保留印刷ＦＧ = (sKey.Length > 7) ? sKey[7] : "0";
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 END

			OracleConnection conn2 = null;
			string[] sRet = new string[2];
			string s更新日時 = System.DateTime.Now.ToString("yyyyMMddHHmmss");

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "UPDATE ＣＭ０１会員 \n"
					+   " SET 会員名 = '" + sKey[1] + "' "
					+       ",使用開始日 = '" + sKey[2] + "' " 
					+       ",使用終了日 = '" + sKey[3] + "' "
					+       ",管理者区分 = '" + sKey[4] + "' "
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 START
					+       ",保留印刷ＦＧ = '" + s保留印刷ＦＧ + "' "
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 END
					+       ",更新日時 = " + s更新日時
					+       ",更新ＰＧ = '会員更新' "
					+       ",更新者 = '" + sKey[6] + "' \n"
					+ " WHERE 会員ＣＤ = '" + sKey[0] + "' \n"
					+   " AND 削除ＦＧ = '0' \n"
					+   " AND 更新日時 = " + sKey[5] + " \n";

				if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
					tran.Commit();
					sRet[0] = "正常終了";
					sRet[1] = s更新日時;
				}
				else
				{
					tran.Rollback();
					sRet[0] = "他の端末で更新されています";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 会員マスタ削除
		 * 引数：会員ＣＤ
		 * 戻値：ステータス
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Del_Member(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "会員マスタ削除開始");
// MOD 2010.08.26 東都）高木 お客様、セクション、請求先削除時のログ強化 START
			logWriter(sUser, INF, "お客様削除 ["+sKey[0]+"]");
// MOD 2010.08.26 東都）高木 お客様、セクション、請求先削除時のログ強化 END

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}

// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}

// MOD 2016.04.08 bevas) 松本 社内伝票機能追加対応 START
			//削除対象会員が社内伝会員の場合は、ＣＭ０５会員扱店Ｆテーブルの削除も併せて実行
			if(sKey[0].Substring(0,2).ToUpper() == "FK")
			{
				//ＣＭ０５Ｆの削除対象キーを取得
				string[] sRet2 = new string[1];
				sRet2 = this.Sel_HouseSlipMember(sUser, sKey);
				if(!sRet2[0].Equals("正常終了"))
				{
					sRet[0] = "【社内伝会員扱店削除エラー】" + sRet2[0];
					return sRet;
				}

				//ＣＭ０５Ｆを削除
				string[] sKey2 = new string[3];
				sKey2[0] = sKey[0];
				sKey2[1] = sRet2[5].Trim();
				sKey2[2] = sKey[2];
				string[] sRet3 = new string[1];
				sRet3 = this.Del_HouseSlipMember(sUser, sKey2);
				if(!sRet3[0].Equals("正常終了"))
				{
					sRet[0] = "【社内伝会員扱店削除エラー】" + sRet3[0];
					return sRet;
				}
			}
// MOD 2016.04.08 bevas) 松本 社内伝票機能追加対応 END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "UPDATE ＣＭ０１会員 \n"
					+    "SET 削除ＦＧ = '1' "
					+       ",更新日時 = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
					+       ",更新ＰＧ = '会員更新' "
					+       ",更新者 = '" + sKey[2] + "' \n"
					+ " WHERE 会員ＣＤ = '" + sKey[0] + "' "
					+   " AND 更新日時 = " + sKey[1] + " \n";

				if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
					tran.Commit();
					sRet[0] = "正常終了";
				}
				else
				{
					tran.Rollback();
					sRet[0] = "他の端末で更新されています";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 部門マスタ検索
		 * 引数：会員ＣＤ、部門ＣＤ
		 * 戻値：ステータス、部門ＣＤ、部門名、出力順、店所名、更新日時
		 *
		 * 参照元：会員マスタ.cs 2回
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_Section(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "部門マスタ検索開始");

			OracleConnection conn2 = null;
// MOD 2009.03.03 東都）高木 サーマルシリアル番号の追加 START
//			string[] sRet = new string[10];
			string[] sRet = new string[19];
// MOD 2009.03.03 東都）高木 サーマルシリアル番号の追加 END

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT CM02.部門ＣＤ "
					+      ", CM02.部門名 "
					+      ", CM02.出力順 "
					+      ", CM02.郵便番号 "
					+      ", NVL(CM10.店所名, ' ') "
					+      ", CM02.設置先住所１ "
					+      ", CM02.設置先住所２ "
					+      ", CM02.更新日時 \n"
					+      ", CM02.サーマル台数 \n"
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） START
//// MOD 2009.03.03 東都）高木 サーマルシリアル番号の追加 START
//					+      ", CM02.シリアル番号１ \n"
//					+      ", CM02.状態１ \n"
//					+      ", CM02.シリアル番号２ \n"
//					+      ", CM02.状態２ \n"
//					+      ", CM02.シリアル番号３ \n"
//					+      ", CM02.状態３ \n"
//					+      ", CM02.シリアル番号４ \n"
//					+      ", CM02.状態４ \n"
//					+      ", CM02.使用料 \n"
//// MOD 2009.03.03 東都）高木 サーマルシリアル番号の追加 END
					+      ", NVL(CM06.シリアル番号１,' ') \n"
					+      ", NVL(CM06.状態１,' ') \n"
					+      ", NVL(CM06.シリアル番号２,' ') \n"
					+      ", NVL(CM06.状態２,' ') \n"
					+      ", NVL(CM06.シリアル番号３,' ') \n"
					+      ", NVL(CM06.状態３,' ') \n"
					+      ", NVL(CM06.シリアル番号４,' ') \n"
					+      ", NVL(CM06.状態４,' ') \n"
					+      ", NVL(CM06.使用料,0) \n"
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） END
					+  " FROM ＣＭ０２部門 CM02 \n"
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） START
					+      " LEFT JOIN ＣＭ０６部門拡張 CM06 \n"
					+      " ON CM02.会員ＣＤ = CM06.会員ＣＤ \n"
					+      " AND CM02.部門ＣＤ = CM06.部門ＣＤ \n"
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） END
// MOD 2009.06.23 東都）高木 住所マスタのプライマリーキー変更 START
//					+  " LEFT JOIN ＣＭ１３住所 CM13 \n"
//					+    " ON CM02.郵便番号 = CM13.郵便番号 "
//					+   " AND CM13.削除ＦＧ = '0' \n"
//					+  " LEFT JOIN ＣＭ１０店所 CM10 \n"
//					+    " ON CM13.店所ＣＤ = CM10.店所ＣＤ "
//					+   " AND CM10.削除ＦＧ = '0' \n"
					+  " LEFT JOIN ＣＭ１４郵便番号 CM14 \n"
					+    " ON CM02.郵便番号 = CM14.郵便番号 "
//注意 MOD 2010.04.13 東都）高木 郵便番号が削除された時の障害対応 START
//					+   " AND CM14.削除ＦＧ = '0' \n"
//注意 MOD 2010.04.13 東都）高木 郵便番号が削除された時の障害対応 END
					+  " LEFT JOIN ＣＭ１０店所 CM10 \n"
					+    " ON CM14.店所ＣＤ = CM10.店所ＣＤ "
					+   " AND CM10.削除ＦＧ = '0' \n"
// MOD 2009.06.23 東都）高木 住所マスタのプライマリーキー変更 END
					+ " WHERE CM02.会員ＣＤ = '" + sKey[0] + "' \n"
					+   " AND CM02.部門ＣＤ = '" + sKey[1] + "' \n"
					+   " AND CM02.削除ＦＧ = '0' \n"
					;

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				while (reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();
					sRet[2] = reader.GetString(1).Trim();
					sRet[3] = reader.GetDecimal(2).ToString().Trim();
					sRet[4] = reader.GetString(3).Trim();
					sRet[5] = reader.GetString(4).Trim();
					sRet[6] = reader.GetString(5).Trim();
					sRet[7] = reader.GetString(6).Trim();
					sRet[8] = reader.GetDecimal(7).ToString().Trim();
					sRet[9] = reader.GetDecimal(8).ToString().Trim();
// ADD 2009.03.03 東都）高木 サーマルシリアル番号の追加 START
					sRet[10] = reader.GetString(9).Trim();
					sRet[11] = reader.GetString(10).Trim();
					sRet[12] = reader.GetString(11).Trim();
					sRet[13] = reader.GetString(12).Trim();
					sRet[14] = reader.GetString(13).Trim();
					sRet[15] = reader.GetString(14).Trim();
					sRet[16] = reader.GetString(15).Trim();
					sRet[17] = reader.GetString(16).Trim();
					sRet[18] = reader.GetDecimal(17).ToString().Trim();
// ADD 2009.03.03 東都）高木 サーマルシリアル番号の追加 END
					iCnt++;
				}
// ADD 2009.03.03 東都）高木 サーマルシリアル番号の追加 START
				if(sRet[11].Trim().Length == 0) sRet[11] = "0"; 
				if(sRet[13].Trim().Length == 0) sRet[13] = "0"; 
				if(sRet[15].Trim().Length == 0) sRet[15] = "0"; 
				if(sRet[17].Trim().Length == 0) sRet[17] = "0"; 
				if(sRet[18].Trim().Length == 0) sRet[18] = "0"; 
// ADD 2009.03.03 東都）高木 サーマルシリアル番号の追加 END
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				if(iCnt == 1) 
					sRet[0] = "該当データがありません";
				else
					sRet[0] = "正常終了";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 部門マスタ件数検索
		 * 引数：会員ＣＤ、郵便番号
		 * 戻値：ステータス、件数
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_SectionCount(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "部門数検索開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[2];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT COUNT(部門ＣＤ) "
//					= "SELECT NVL(COUNT(部門ＣＤ),0) "
					+   "FROM ＣＭ０２部門 CM02 "
					+  "WHERE 会員ＣＤ = '" + sKey[0] + "' "
					+    "AND 郵便番号 = '" + sKey[1] + "' "
					+    "AND 削除ＦＧ = '0' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				while (reader.Read())
				{
					sRet[1] = reader.GetDecimal(0).ToString().Trim();
					iCnt++;
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				if(iCnt == 1) 
					sRet[0] = "該当データがありません";
				else
					sRet[0] = "正常終了";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 部門マスタ一覧取得
		 * 引数：会員ＣＤ、部門ＣＤ、部門名
		 * 戻値：ステータス、部門ＣＤ、部門名、出力順、郵便番号
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Get_Section(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "部門マスタ一覧取得開始");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' "
					+     "|| TRIM(部門ＣＤ) || '|' "
					+     "|| TRIM(部門名) || '|' "
					+     "|| TRIM(出力順) || '|' "
					+     "|| TRIM(郵便番号) || '|' \n"
					+  " FROM ＣＭ０２部門 \n"
					+ " WHERE 会員ＣＤ = '" + sKey[0] + "' \n";
				if (sKey[1].Trim().Length == 4)
				{
					cmdQuery += " AND 部門ＣＤ = '" + sKey[1] + "' \n";
				}
				else
				{
					cmdQuery += " AND 部門ＣＤ LIKE '" + sKey[1] + "%' \n";
				}
				if (sKey[2].Trim().Length != 0)
				{
					cmdQuery += " AND 部門名 LIKE '" + sKey[2] + "%' \n";
				}
				cmdQuery += " AND 削除ＦＧ = '0' \n";
				cmdQuery += " ORDER BY 部門ＣＤ \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "該当データがありません";
				else
				{
					sRet[0] = "正常終了";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 部門マスタ追加
		 * 引数：会員ＣＤ、部門ＣＤ...
		 * 戻値：ステータス
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Ins_Section(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "部門マスタ追加開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT 削除ＦＧ "
					+   "FROM ＣＭ０２部門 "
					+  "WHERE 会員ＣＤ = '" + sKey[0] + "' "
					+    "AND 部門ＣＤ = '" + sKey[1] + "' "
					+    "FOR UPDATE \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				string s削除ＦＧ = "";
				while (reader.Read())
				{
					s削除ＦＧ = reader.GetString(0);
					iCnt++;
				}
				if(iCnt == 1)
				{
					//追加
					cmdQuery
						= "INSERT INTO ＣＭ０２部門 \n"
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） START
//// ADD 2009.03.03 東都）高木 サーマルシリアル番号の追加 START
//						+         "(会員ＣＤ \n"
//						+         ",部門ＣＤ \n"
//						+         ",部門名 \n"
//						+         ",組織ＣＤ \n"
//						+         ",出力順 \n"
//						+         ",郵便番号 \n"
//						+         ",\"ジャーナルＮＯ登録日\" \n"
//						+         ",\"ジャーナルＮＯ管理\" \n"
//						+         ",雛型ＮＯ \n"
//						+         ",出荷日 \n"
//						+         ",設置先住所１ \n"
//						+         ",設置先住所２ \n"
//						+         ",サーマル台数 \n"
//						;
//						if(sKey.Length > 10){
//							cmdQuery = cmdQuery
//							+     ",シリアル番号１ \n"
//							+     ",状態１ \n"
//							+     ",シリアル番号２ \n"
//							+     ",状態２ \n"
//							+     ",シリアル番号３ \n"
//							+     ",状態３ \n"
//							+     ",シリアル番号４ \n"
//							+     ",状態４ \n"
//							+     ",使用料 \n"
//							;
//						}
//					cmdQuery = cmdQuery
//						+         ",削除ＦＧ \n"
//						+         ",登録日時 \n"
//						+         ",登録ＰＧ \n"
//						+         ",登録者 \n"
//						+         ",更新日時 \n"
//						+         ",更新ＰＧ \n"
//						+         ",更新者 \n"
//						+         ") \n"
//// ADD 2009.03.03 東都）高木 サーマルシリアル番号の追加 END
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） END
						+ " VALUES ('" + sKey[0] + "' " 
						+         ",'" + sKey[1] + "' "
						+         ",'" + sKey[2] + "' "
						+         ",' ' "
						+         ", " + sKey[3]
						+         ",'" + sKey[4] + "' "
						+         ",TO_CHAR(SYSDATE,'YYYYMMDD') " // ジャーナルＮＯ登録日
						+         ", 0 "							 // ジャーナル管理ＮＯ
						+         ", 0 "							 // 雛型ＮＯ
						+         ",TO_CHAR(SYSDATE,'YYYYMMDD') " // 出荷日
						+         ",'" + sKey[5] + "' "
						+         ",'" + sKey[6] + "' "
						+         ", " + sKey[9] + "  "
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） START
//// ADD 2009.03.03 東都）高木 サーマルシリアル番号の追加 START
//						;
//						if(sKey.Length > 10){
//							cmdQuery = cmdQuery
//							+     ",'" + sKey[10] + "' "
//							+     ",'" + sKey[11] + "' "
//							+     ",'" + sKey[12] + "' "
//							+     ",'" + sKey[13] + "' "
//							+     ",'" + sKey[14] + "' "
//							+     ",'" + sKey[15] + "' "
//							+     ",'" + sKey[16] + "' "
//							+     ",'" + sKey[17] + "' "
//							+     ", " + sKey[18] + "  "
//							;
//						}
//					cmdQuery = cmdQuery
//// ADD 2009.03.03 東都）高木 サーマルシリアル番号の追加 END
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） END
						+         ",'0' "
						+         ",TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
						+         ",'会員登録' "
						+         ",'" + sKey[8] + "' "
						+         ",TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
						+         ",'会員登録' "
						+         ",'" + sKey[8] + "' "
						+ " ) \n";

					CmdUpdate(sUser, conn2, cmdQuery);
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） START
					if(sKey.Length > 10){
						cmdQuery
							= "INSERT INTO ＣＭ０６部門拡張 \n"
							+         "(会員ＣＤ \n"
							+         ",部門ＣＤ \n"
							+     ",シリアル番号１ \n"
							+     ",状態１ \n"
							+     ",シリアル番号２ \n"
							+     ",状態２ \n"
							+     ",シリアル番号３ \n"
							+     ",状態３ \n"
							+     ",シリアル番号４ \n"
							+     ",状態４ \n"
							+     ",使用料 \n"
							+         ",削除ＦＧ \n"
							+         ",登録日時 \n"
							+         ",登録ＰＧ \n"
							+         ",登録者 \n"
							+         ",更新日時 \n"
							+         ",更新ＰＧ \n"
							+         ",更新者 \n"
							+         ") \n"
							+ " VALUES ('" + sKey[0] + "' " 
							+         ",'" + sKey[1] + "' "
							+     ",'" + sKey[10] + "' "
							+     ",'" + sKey[11] + "' "
							+     ",'" + sKey[12] + "' "
							+     ",'" + sKey[13] + "' "
							+     ",'" + sKey[14] + "' "
							+     ",'" + sKey[15] + "' "
							+     ",'" + sKey[16] + "' "
							+     ",'" + sKey[17] + "' "
							+     ", " + sKey[18] + "  "
							+         ",'0' "
							+         ",TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
							+         ",'会員登録' "
							+         ",'" + sKey[8] + "' "
							+         ",TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
							+         ",'会員登録' "
							+         ",'" + sKey[8] + "' "
							+ " ) \n";

						CmdUpdate(sUser, conn2, cmdQuery);
					}
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） END
					sRet[0] = "正常終了";
				}
				else
				{
					//追加更新
					if (s削除ＦＧ.Equals("1"))
					{
						cmdQuery
							= "UPDATE ＣＭ０２部門 \n"
							+   " SET 部門名 = '" + sKey[2] + "' "
							+       ",組織ＣＤ = ' ' "
							+       ",出力順 = " + sKey[3] 
							+       ",郵便番号 = '" + sKey[4] + "' "
							+       ",ジャーナルＮＯ登録日 = TO_CHAR(SYSDATE,'YYYYMMDD') "
							+       ",ジャーナルＮＯ管理 = 0 "
							+       ",雛型ＮＯ = 0 "
							+       ",出荷日 = TO_CHAR(SYSDATE,'YYYYMMDD') "
							+       ",設置先住所１ = '" + sKey[5] + "' "
							+       ",設置先住所２ = '" + sKey[6] + "' "
							+       ",サーマル台数 =  " + sKey[9] + "  "
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） START
//// ADD 2009.03.03 東都）高木 サーマルシリアル番号の追加 START
//							;
//							if(sKey.Length > 10){
//								cmdQuery = cmdQuery
//								+     ",シリアル番号１ = '" + sKey[10] + "' \n"
//								+     ",状態１ = '" + sKey[11] + "' \n"
//								+     ",シリアル番号２ = '" + sKey[12] + "' \n"
//								+     ",状態２ = '" + sKey[13] + "' \n"
//								+     ",シリアル番号３ = '" + sKey[14] + "' \n"
//								+     ",状態３ = '" + sKey[15] + "' \n"
//								+     ",シリアル番号４ = '" + sKey[16] + "' \n"
//								+     ",状態４ = '" + sKey[17] + "' \n"
//								+     ",使用料 = " + sKey[18] + "  \n"
//								+     ",会員申込管理番号 = 0 \n"
//								+     ",荷受人繰越方法 = ' ' \n"
//								+     ",荷受人繰越年月 = ' ' \n"
//								+     ",フラグ１ = ' ' \n"
//								+     ",フラグ２ = ' ' \n"
//								+     ",フラグ３ = ' ' \n"
//								;
//							}
//						cmdQuery = cmdQuery
//// ADD 2009.03.03 東都）高木 サーマルシリアル番号の追加 END
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） END
							+       ",削除ＦＧ = '0' "
							+       ",登録日時 = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
							+       ",登録ＰＧ = '会員登録' "
							+       ",登録者 = '" + sKey[8] + "' "
							+       ",更新日時 = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
							+       ",更新ＰＧ = '会員登録' \n"
							+       ",更新者 = '" + sKey[8] + "'"
							+ " WHERE 会員ＣＤ = '" + sKey[0] + "' \n"
							+   " AND 部門ＣＤ = '" + sKey[1] + "' \n";

						CmdUpdate(sUser, conn2, cmdQuery);
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） START
						if(sKey.Length > 10){
							cmdQuery
								= "UPDATE ＣＭ０６部門拡張 SET \n"
							+     " シリアル番号１ = '" + sKey[10] + "' \n"
							+     ",状態１ = '" + sKey[11] + "' \n"
							+     ",シリアル番号２ = '" + sKey[12] + "' \n"
							+     ",状態２ = '" + sKey[13] + "' \n"
							+     ",シリアル番号３ = '" + sKey[14] + "' \n"
							+     ",状態３ = '" + sKey[15] + "' \n"
							+     ",シリアル番号４ = '" + sKey[16] + "' \n"
							+     ",状態４ = '" + sKey[17] + "' \n"
							+     ",使用料 = " + sKey[18] + "  \n"
							+     ",会員申込管理番号 = 0 \n"
							+     ",荷受人繰越方法 = ' ' \n"
							+     ",荷受人繰越年月 = ' ' \n"
							+     ",フラグ１ = ' ' \n"
							+     ",フラグ２ = ' ' \n"
							+     ",フラグ３ = ' ' \n"
							+       ",削除ＦＧ = '0' "
							+       ",登録日時 = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
							+       ",登録ＰＧ = '会員登録' "
							+       ",登録者 = '" + sKey[8] + "' "
							+       ",更新日時 = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
							+       ",更新ＰＧ = '会員登録' \n"
							+       ",更新者 = '" + sKey[8] + "'"
							+ " WHERE 会員ＣＤ = '" + sKey[0] + "' \n"
							+   " AND 部門ＣＤ = '" + sKey[1] + "' \n";

							CmdUpdate(sUser, conn2, cmdQuery);
						}
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） END
						sRet[0] = "正常終了";
					}
					else
					{
						sRet[0] = "既に登録されています";
					}
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				logWriter(sUser, INF, sRet[0]);

				if (sRet[0].Equals("正常終了"))
				{
					logWriter(sUser, INF, "記事の初期データ登録開始");

					//記事の初期データの検索
					cmdQuery
						= "SELECT 記事ＣＤ "
						+      ", 記事 "
						+   "FROM ＳＭ０３記事 "
						+  "WHERE 会員ＣＤ = 'default' "
						+    "AND 部門ＣＤ = '0000' "
						+    "AND 削除ＦＧ = '0' \n";

					OracleDataReader readerDef = CmdSelect(sUser, conn2, cmdQuery);
					string s初期記事ＣＤ = "";
					string s初期記事     = "";
					while (readerDef.Read())
					{
						s初期記事ＣＤ = readerDef.GetString(0);
						s初期記事     = readerDef.GetString(1);

						//記事の検索
						cmdQuery
							= "SELECT * "
							+   "FROM ＳＭ０３記事 "
							+  "WHERE 会員ＣＤ = '" + sKey[0] + "' "
							+    "AND 部門ＣＤ = '" + sKey[1] + "' "
							+    "AND 記事ＣＤ = '" + s初期記事ＣＤ + "' "
						    +    "FOR UPDATE \n";

						OracleDataReader readerNote = CmdSelect(sUser, conn2, cmdQuery);
						if (readerNote.Read())
						{
							//既に記事がある場合は新規更新
							cmdQuery
								= "UPDATE ＳＭ０３記事 \n"
								+   " SET 記事 = '" + s初期記事 + "' "
								+       ",削除ＦＧ = '0' "
								+       ",登録日時 = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
								+       ",登録ＰＧ = '初期記事' "
								+       ",登録者 = '" + sKey[8] + "' "
								+       ",更新日時 = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
								+       ",更新ＰＧ = '初期記事' "
								+       ",更新者 = '" + sKey[8] + "' \n"
								+ " WHERE 会員ＣＤ = '" + sKey[0] + "' \n"
								+   " AND 部門ＣＤ = '" + sKey[1] + "' \n"
								+   " AND 記事ＣＤ = '" + s初期記事ＣＤ + "' \n";

							CmdUpdate(sUser, conn2, cmdQuery);
							sRet[0] = "正常終了";
						}
						else
						{
							//新規追加
							cmdQuery
								= "INSERT INTO ＳＭ０３記事 \n"
								+ " VALUES ('" + sKey[0] + "' " 
								+         ",'" + sKey[1] + "' "
								+         ",'" + s初期記事ＣＤ + "' "
								+         ",'" + s初期記事 + "' "
								+         ",'0' "
								+         ",TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
								+         ",'初期記事' "
								+         ",'" + sKey[8] + "' "
								+         ",TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
								+         ",'初期記事' "
								+         ",'" + sKey[8] + "' "
								+ " ) \n";

							CmdUpdate(sUser, conn2, cmdQuery);
							sRet[0] = "正常終了";
						}
						logWriter(sUser, INF, sRet[0]);
					}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
					disposeReader(reader);
					reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				}
				tran.Commit();
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 部門マスタ更新
		 * 引数：会員ＣＤ、部門ＣＤ...
		 * 戻値：ステータス
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Upd_Section(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "部門マスタ更新開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "UPDATE ＣＭ０２部門 \n"
					+   " SET 部門名 = '" + sKey[2] + "' "
					+       ",出力順 =  " + sKey[3] 
					+       ",郵便番号 = '" + sKey[4] + "' "
					+       ",設置先住所１ = '" + sKey[5] + "' "
					+       ",設置先住所２ = '" + sKey[6] + "' "
					+       ",サーマル台数 =  " + sKey[9] + "  "
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） START
//// ADD 2009.03.03 東都）高木 サーマルシリアル番号の追加 START
//					;
//					if(sKey.Length > 10){
//						cmdQuery = cmdQuery
//						+     ",シリアル番号１ = '" + sKey[10] + "' \n"
//						+     ",状態１ = '" + sKey[11] + "' \n"
//						+     ",シリアル番号２ = '" + sKey[12] + "' \n"
//						+     ",状態２ = '" + sKey[13] + "' \n"
//						+     ",シリアル番号３ = '" + sKey[14] + "' \n"
//						+     ",状態３ = '" + sKey[15] + "' \n"
//						+     ",シリアル番号４ = '" + sKey[16] + "' \n"
//						+     ",状態４ = '" + sKey[17] + "' \n"
//						+     ",使用料 = " + sKey[18] + "  \n"
//						;
//					}
//				cmdQuery = cmdQuery
//// ADD 2009.03.03 東都）高木 サーマルシリアル番号の追加 END
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） END
					+       ",更新日時 = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
					+       ",更新ＰＧ = '会員更新' "
					+       ",更新者 = '" + sKey[8] + "' \n"
					+ " WHERE 会員ＣＤ = '" + sKey[0] + "' \n"
					+   " AND 部門ＣＤ = '" + sKey[1] + "' \n"
					+   " AND 削除ＦＧ = '0' \n"
					+   " AND 更新日時 =  " + sKey[7] + " \n";

				if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） START
					if(sKey.Length > 10){
						cmdQuery
						= "UPDATE ＣＭ０６部門拡張 SET \n"
						+     " シリアル番号１ = '" + sKey[10] + "' \n"
						+     ",状態１ = '" + sKey[11] + "' \n"
						+     ",シリアル番号２ = '" + sKey[12] + "' \n"
						+     ",状態２ = '" + sKey[13] + "' \n"
						+     ",シリアル番号３ = '" + sKey[14] + "' \n"
						+     ",状態３ = '" + sKey[15] + "' \n"
						+     ",シリアル番号４ = '" + sKey[16] + "' \n"
						+     ",状態４ = '" + sKey[17] + "' \n"
						+     ",使用料 = " + sKey[18] + "  \n"
						+       ",更新日時 = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
						+       ",更新ＰＧ = '会員更新' "
						+       ",更新者 = '" + sKey[8] + "' \n"
						+ " WHERE 会員ＣＤ = '" + sKey[0] + "' \n"
						+   " AND 部門ＣＤ = '" + sKey[1] + "' \n"
						+   " AND 削除ＦＧ = '0' \n"
						;
//						+   " AND 更新日時 =  " + sKey[7] + " \n";

						if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
						{
							;
						}
					}
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） END
					tran.Commit();
					sRet[0] = "正常終了";
				}
				else
				{
					tran.Rollback();
					sRet[0] = "他の端末で更新されています";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 部門マスタ削除
		 * 引数：会員ＣＤ、部門ＣＤ...
		 * 戻値：ステータス
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Del_Section(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "部門マスタ削除開始");
// MOD 2010.08.26 東都）高木 お客様、セクション、請求先削除時のログ強化 START
			logWriter(sUser, INF, "セクション削除　["+sKey[0]+"]["+sKey[1]+"]");
// MOD 2010.08.26 東都）高木 お客様、セクション、請求先削除時のログ強化 END

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "UPDATE ＣＭ０２部門 \n"
					+    "SET 削除ＦＧ = '1' "
					+       ",更新日時 = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
					+       ",更新ＰＧ = '会員更新' "
					+       ",更新者 = '" + sKey[3] + "' \n"
					+ " WHERE 会員ＣＤ = '" + sKey[0] + "' "
					+   " AND 部門ＣＤ = '" + sKey[1] + "' "
					+   " AND 更新日時 = " + sKey[2] + " \n";

				if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） START
					cmdQuery
						= "UPDATE ＣＭ０６部門拡張 \n"
						+    "SET 削除ＦＧ = '1' "
						+       ",更新日時 = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
						+       ",更新ＰＧ = '会員更新' "
						+       ",更新者 = '" + sKey[3] + "' \n"
						+ " WHERE 会員ＣＤ = '" + sKey[0] + "' "
						+   " AND 部門ＣＤ = '" + sKey[1] + "' ";

					if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
					{
						;
					}
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） END
					tran.Commit();
					sRet[0] = "正常終了";
				}
				else
				{
					tran.Rollback();
					sRet[0] = "他の端末で更新されています";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 利用者マスタ検索
		 * 引数：会員ＣＤ、利用者ＣＤ
		 * 戻値：ステータス、利用者ＣＤ、パスワード、利用者名...
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_User(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "利用者マスタ検索開始");

			OracleConnection conn2 = null;
// MOD 2005.07.21 東都）高木 店所ユーザ対応 START
//			string[] sRet = new string[9];
// MOD 2008.05.29 東都）高木 パスワード更新年月日を表示 START
//			string[] sRet = new string[10];
			string[] sRet = new string[11];
// MOD 2008.05.29 東都）高木 パスワード更新年月日を表示 END
// MOD 2005.07.21 東都）高木 店所ユーザ対応 END

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT CM04.利用者ＣＤ "
					+       ",CM04.\"パスワード\" "
					+       ",CM04.利用者名 "
					+       ",CM04.部門ＣＤ "
					+       ",NVL(CM02.部門名, ' ') "
					+       ",CM04.荷送人ＣＤ "
					+       ",CM04.認証エラー回数 "
					+       ",CM04.更新日時 \n"
// ADD 2005.07.21 東都）高木 店所ユーザ対応 START
					+       ",CM04.権限１ \n"
// ADD 2005.07.21 東都）高木 店所ユーザ対応 END
// ADD 2008.05.29 東都）高木 パスワード更新年月日を表示 START
					+       ",CM04.登録ＰＧ \n"
// ADD 2008.05.29 東都）高木 パスワード更新年月日を表示 END
					+  " FROM ＣＭ０４利用者 CM04 \n"
					+  " LEFT JOIN  ＣＭ０２部門 CM02 \n"
					+    " ON CM04.会員ＣＤ = CM02.会員ＣＤ "
					+    "AND CM04.部門ＣＤ = CM02.部門ＣＤ "
					+    "AND CM02.削除ＦＧ = '0' \n"
					+ " WHERE CM04.会員ＣＤ = '" + sKey[0] + "' \n"
					+   " AND CM04.利用者ＣＤ = '" + sKey[1] + "' \n"
					+   " AND CM04.削除ＦＧ   = '0' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				while (reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();
					sRet[2] = reader.GetString(1).Trim();
					sRet[3] = reader.GetString(2).Trim();
					sRet[4] = reader.GetString(3).Trim();
					sRet[5] = reader.GetString(4).Trim();
					sRet[6] = reader.GetString(5).Trim();
					sRet[7] = reader.GetDecimal(6).ToString().Trim();
					sRet[8] = reader.GetDecimal(7).ToString().Trim();
// ADD 2005.07.21 東都）高木 店所ユーザ対応 START
					sRet[9] = reader.GetString(8).Trim();
// ADD 2005.07.21 東都）高木 店所ユーザ対応 END
// ADD 2008.05.29 東都）高木 パスワード更新年月日を表示 START
					sRet[10] = reader.GetString(9).Trim();
// ADD 2008.05.29 東都）高木 パスワード更新年月日を表示 END
					iCnt++;
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				if(iCnt == 1) 
					sRet[0] = "該当データがありません";
				else
					sRet[0] = "正常終了";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

// MOD 2008.12.08 東都）高木 営業所でのパスワード照会対応 START
		/*********************************************************************
		 * 利用者マスタ検索２
		 * 引数：会員ＣＤ、利用者ＣＤ
		 * 戻値：ステータス、利用者ＣＤ、パスワード、利用者名...
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_User2(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "利用者マスタ検索２開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[14];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT CM04.利用者ＣＤ "
					+       ",CM04.\"パスワード\" "
					+       ",CM04.利用者名 "
					+       ",CM04.部門ＣＤ "
					+       ",NVL(CM02.部門名, ' ') "
					+       ",CM04.荷送人ＣＤ "
					+       ",CM04.認証エラー回数 "
					+       ",CM04.更新日時 \n"
					+       ",CM04.権限１ \n"
					+       ",CM04.登録ＰＧ \n"
					+       ",NVL(CM02.郵便番号, ' ') "
					+       ",NVL(CM14.店所ＣＤ, ' ') "
					+       ",NVL(CM10.店所名, ' ') "
					+  " FROM ＣＭ０４利用者 CM04 \n"
					+  " LEFT JOIN  ＣＭ０２部門 CM02 \n"
					+    " ON CM04.会員ＣＤ = CM02.会員ＣＤ "
					+    "AND CM04.部門ＣＤ = CM02.部門ＣＤ "
					+    "AND CM02.削除ＦＧ = '0' \n"
					+  " LEFT JOIN ＣＭ１４郵便番号 CM14 \n"
					+    " ON CM02.郵便番号 = CM14.郵便番号 "
//					+   " AND CM14.削除ＦＧ = '0' \n"
					+  " LEFT JOIN ＣＭ１０店所 CM10 \n"
					+    " ON CM14.店所ＣＤ = CM10.店所ＣＤ "
//					+   " AND CM10.削除ＦＧ = '0' \n"
					+ " WHERE CM04.会員ＣＤ = '" + sKey[0] + "' \n"
					+   " AND CM04.利用者ＣＤ = '" + sKey[1] + "' \n"
					+   " AND CM04.削除ＦＧ   = '0' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				while (reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();
					sRet[2] = reader.GetString(1).Trim();
					sRet[3] = reader.GetString(2).Trim();
					sRet[4] = reader.GetString(3).Trim();
					sRet[5] = reader.GetString(4).Trim();
					sRet[6] = reader.GetString(5).Trim();
					sRet[7] = reader.GetDecimal(6).ToString().Trim();
					sRet[8] = reader.GetDecimal(7).ToString().Trim();
					sRet[9] = reader.GetString(8).Trim();
					sRet[10] = reader.GetString(9).Trim();
					sRet[11] = reader.GetString(10).Trim();
					sRet[12] = reader.GetString(11).Trim();
					sRet[13] = reader.GetString(12).Trim();

					iCnt++;
				}
				disposeReader(reader);
				reader = null;

				if(iCnt == 1) 
					sRet[0] = "該当データがありません";
				else
					sRet[0] = "正常終了";

				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}
// MOD 2008.12.08 東都）高木 営業所でのパスワード照会対応 END

		/*********************************************************************
		 * 利用者マスタ一覧取得
		 * 引数：会員ＣＤ、利用者ＣＤ、利用者名
		 * 戻値：ステータス、利用者ＣＤ、利用者名、部門ＣＤ、荷送人ＣＤ
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Get_User(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "利用者マスタ一覧取得開始");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' "
					+     "|| TRIM(利用者ＣＤ) || '|' "
					+     "|| TRIM(利用者名) || '|' "
					+     "|| TRIM(部門ＣＤ) || '|' "
					+     "|| TRIM(荷送人ＣＤ) || '|' \n"
					+  " FROM ＣＭ０４利用者 \n"
					+ " WHERE 会員ＣＤ = '" + sKey[0] + "' \n";
				if (sKey[1].Trim().Length == 4)
				{
					cmdQuery += " AND 利用者ＣＤ = '" + sKey[1] + "' \n";
				}
				else
				{
					cmdQuery += " AND 利用者ＣＤ LIKE '" + sKey[1] + "%' \n";
				}
				if (sKey[2].Trim().Length != 0)
				{
					cmdQuery += " AND 利用者名 LIKE '" + sKey[2] + "%' \n";
				}
// ADD 2005.06.13 東都）小童谷 部門ＣＤ追加 START
				if (sKey[3].Trim().Length != 0)
				{
					cmdQuery += " AND 部門ＣＤ = '" + sKey[3] + "' \n";
				}
// ADD 2005.06.13 東都）小童谷 部門ＣＤ追加 END
				cmdQuery += " AND 削除ＦＧ = '0' \n";
				cmdQuery += " ORDER BY 利用者ＣＤ \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				sRet = new string[sList.Count + 1];

				if(sList.Count == 0) 
					sRet[0] = "該当データがありません";
				else
				{
					sRet[0] = "正常終了";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

// MOD 2008.12.08 東都）高木 営業所でのパスワード照会対応 START
		/*********************************************************************
		 * 利用者マスタ一覧取得２
		 * 引数：会員ＣＤ、店所ＣＤ
		 * 戻値：ステータス、部門ＣＤ、部門名、利用者ＣＤ、利用者名
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Get_User2(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "利用者マスタ一覧取得２開始");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' "
					+     "|| ' ' || TRIM(CM04.部門ＣＤ) || '|' "
					+     "|| TRIM(CM02.部門名) || '|' "
					+     "|| ' ' || TRIM(CM04.利用者ＣＤ) || '|' "
					+     "|| TRIM(CM04.利用者名) || '|' "
					+  " FROM ＣＭ０４利用者 CM04 \n"
					+      ", ＣＭ０２部門 CM02 \n"
					+      ", ＣＭ１４郵便番号 CM14 \n"
					+ " WHERE CM04.会員ＣＤ = '" + sKey[0] + "' \n";
				cmdQuery += " AND CM04.削除ＦＧ = '0' \n";
				cmdQuery += " AND CM04.会員ＣＤ = CM02.会員ＣＤ \n";
				cmdQuery += " AND CM04.部門ＣＤ = CM02.部門ＣＤ \n";
				cmdQuery += " AND CM02.削除ＦＧ = '0' \n";
				cmdQuery += " AND CM02.郵便番号 = CM14.郵便番号 \n";
				if (sKey[1].Trim().Length != 0)
				{
					cmdQuery += " AND CM14.店所ＣＤ = '" + sKey[1] + "' \n";
				}
//				cmdQuery += " AND CM14.削除ＦＧ = '0' \n";
				cmdQuery += " ORDER BY CM04.部門ＣＤ, CM04.利用者ＣＤ \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
				disposeReader(reader);
				reader = null;

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "該当データがありません";
				else
				{
					sRet[0] = "正常終了";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}
// MOD 2008.12.08 東都）高木 営業所でのパスワード照会対応 END
		/*********************************************************************
		 * 利用者マスタ追加
		 * 引数：会員ＣＤ、利用者ＣＤ、利用者名
		 * 戻値：ステータス
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Ins_User(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "利用者マスタ追加開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT 削除ＦＧ "
					+   "FROM ＣＭ０４利用者 "
					+  "WHERE 会員ＣＤ = '" + sKey[0] + "' "
					+    "AND 利用者ＣＤ = '" + sKey[1] + "' "
					+    "FOR UPDATE \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				string s削除ＦＧ = "";
				while (reader.Read())
				{
					s削除ＦＧ = reader.GetString(0);
					iCnt++;
				}
				if(iCnt == 1)
				{
					//追加
					cmdQuery
						= "INSERT INTO ＣＭ０４利用者 \n"
						+ " VALUES ('" + sKey[0] + "' " 
						+         ",'" + sKey[1] + "' "
						+         ",'" + sKey[2] + "' "
						+         ",'" + sKey[3] + "' "
						+         ",'" + sKey[4] + "' "
						+         ",'" + sKey[5] + "' "
						+         ","  + sKey[6]
// MOD 2005.07.21 東都）高木 店所ユーザ対応 START
//						+         ",' ' "
						+         ",'" + sKey[9] + "' \n"
// MOD 2005.07.21 東都）高木 店所ユーザ対応 END
						+         ",' ' "
						+         ",' ' "
						+         ",' ' "
						+         ",' ' "
						+         ",' ' "
						+         ",' ' "
						+         ",' ' "
						+         ",' ' "
						+         ",' ' "
						+         ",'0' "
						+         ",TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
// MOD 2008.05.21 東都）高木 ログインエラー回数を５回にする START
//						+         ",'会員登録' "
						+ "\n";
					if(sKey.Length > 10){
						cmdQuery += ",'" + sKey[10] + "' \n";
					}else{
						cmdQuery += ",TO_CHAR(SYSDATE,'YYYYMMDD') \n";
					}
					cmdQuery = cmdQuery
// MOD 2008.05.21 東都）高木 ログインエラー回数を５回にする END
						+         ",'" + sKey[8] + "' "
						+         ",TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
						+         ",'会員登録' "
						+         ",'" + sKey[8] + "' "
						+ " ) \n";

					CmdUpdate(sUser, conn2, cmdQuery);
					tran.Commit();
					sRet[0] = "正常終了";
				}
				else
				{
					//追加更新
					if (s削除ＦＧ.Equals("1"))
					{
						cmdQuery
							= "UPDATE ＣＭ０４利用者 \n"
							+   " SET パスワード = '" + sKey[2] + "' "
							+       ",利用者名 = '" + sKey[3] + "' "
							+       ",部門ＣＤ = '" + sKey[4] + "' "
							+       ",荷送人ＣＤ = '" + sKey[5] + "' "
							+       ",認証エラー回数 = " + sKey[6]
// ADD 2005.07.21 東都）高木 店所ユーザ対応 START
							+       ",権限１ = '" + sKey[9] + "' \n"
// ADD 2005.07.21 東都）高木 店所ユーザ対応 END
							+       ",削除ＦＧ = '0' "
							+       ",登録日時 = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
// MOD 2008.05.21 東都）高木 ログインエラー回数を５回にする START
//							+       ",登録ＰＧ = '会員登録' "
							+ "\n";
						if(sKey.Length > 10){
							cmdQuery += ",登録ＰＧ = '" + sKey[10] + "' \n";
						}else{
							cmdQuery += ",登録ＰＧ = TO_CHAR(SYSDATE,'YYYYMMDD') \n";
						}
						cmdQuery = cmdQuery
// MOD 2008.05.21 東都）高木 ログインエラー回数を５回にする END
							+       ",登録者 = '" + sKey[8] + "' "
							+       ",更新日時 = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
							+       ",更新ＰＧ = '会員登録' "
							+       ",更新者 = '" + sKey[8] + "' \n"
							+ " WHERE 会員ＣＤ = '" + sKey[0] + "' \n"
							+   " AND 利用者ＣＤ = '" + sKey[1] + "' \n";

						CmdUpdate(sUser, conn2, cmdQuery);
						tran.Commit();
						sRet[0] = "正常終了";
					}
					else
					{
						tran.Rollback();
						sRet[0] = "既に登録されています";
					}
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 利用者マスタ更新
		 * 引数：会員ＣＤ、利用者ＣＤ、パスワード...
		 * 戻値：ステータス
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Upd_User(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "利用者マスタ更新開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "UPDATE ＣＭ０４利用者 \n"
					+   " SET パスワード = '" + sKey[2] + "' "
					+       ",利用者名 = '" + sKey[3] + "' " 
					+       ",部門ＣＤ = '" + sKey[4] + "' "
					+       ",荷送人ＣＤ = '" + sKey[5] + "' "
					+       ",認証エラー回数 = " + sKey[6]
// ADD 2005.07.21 東都）高木 店所ユーザ対応 START
					+       ",権限１ = '" + sKey[9] + "' \n"
// ADD 2005.07.21 東都）高木 店所ユーザ対応 END
// ADD 2008.05.21 東都）高木 ログインエラー回数を５回にする START
					;
				if(sKey.Length > 10){
					cmdQuery += ",登録ＰＧ = '" + sKey[10] + "' \n";
				}
				cmdQuery = cmdQuery
// ADD 2008.05.21 東都）高木 ログインエラー回数を５回にする END
					+       ",更新日時 = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
					+       ",更新ＰＧ = '会員更新' "
					+       ",更新者 = '" + sKey[8] + "' \n"
					+ " WHERE 会員ＣＤ = '" + sKey[0] + "' \n"
					+   " AND 利用者ＣＤ = '" + sKey[1] + "' \n"
					+   " AND 削除ＦＧ = '0' \n"
					+   " AND 更新日時 =  " + sKey[7] + " \n";

				if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
					tran.Commit();
					sRet[0] = "正常終了";
				}
				else
				{
					tran.Rollback();
					sRet[0] = "他の端末で更新されています";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

// MOD 2008.12.08 東都）高木 営業所でのパスワード照会対応 START
		/*********************************************************************
		 * 利用者マスタ更新
		 * 引数：会員ＣＤ、利用者ＣＤ、パスワード...
		 * 戻値：ステータス
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Upd_User2(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "利用者マスタ更新２開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "UPDATE ＣＭ０４利用者 \n"
					+   " SET 認証エラー回数 = " + sKey[3] + " \n"
					+       ",登録ＰＧ = '" + sKey[4] + "' \n"
					+       ",更新日時 = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') \n"
					+       ",更新ＰＧ = '" + sKey[5] + "' \n"
					+       ",更新者 = '" + sKey[6] + "' \n"
					+ " WHERE 会員ＣＤ = '" + sKey[0] + "' \n"
					+   " AND 利用者ＣＤ = '" + sKey[1] + "' \n"
					+   " AND 削除ＦＧ = '0' \n"
					+   " AND 更新日時 =  " + sKey[2] + " \n";

				if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
					tran.Commit();
					sRet[0] = "正常終了";
				}
				else
				{
					tran.Rollback();
					sRet[0] = "他の端末で更新されています";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}
// MOD 2008.12.08 東都）高木 営業所でのパスワード照会対応 END

		/*********************************************************************
		 * 利用者マスタ削除
		 * 引数：会員ＣＤ、利用者ＣＤ、更新日時、更新者
		 * 戻値：ステータス
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Del_User(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "利用者マスタ削除開始");
// MOD 2010.08.26 東都）高木 お客様、セクション、請求先削除時のログ強化 START
			logWriter(sUser, INF, "ユーザ削除 ["+sKey[0]+"]["+sKey[1]+"]");
// MOD 2010.08.26 東都）高木 お客様、セクション、請求先削除時のログ強化 END

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "UPDATE ＣＭ０４利用者 \n"
					+   " SET 削除ＦＧ = '1' \n"
					+       ",更新日時 = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
					+       ",更新ＰＧ = '会員更新' "
					+       ",更新者 = '" + sKey[3] + "' \n"
					+ " WHERE 会員ＣＤ = '" + sKey[0] + "' \n"
					+   " AND 利用者ＣＤ = '" + sKey[1] + "' \n"
					+   " AND 更新日時 = " + sKey[2] + " \n";

				if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
					tran.Commit();
					sRet[0] = "正常終了";
				}
				else
				{
					tran.Rollback();
					sRet[0] = "他の端末で更新されています";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 店所マスタ取得
		 * 引数：店所ＣＤ
		 * 戻値：ステータス、店所名
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_Shop(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "店所マスタ検索開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[2];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT 店所名 "
					+   "FROM ＣＭ１０店所 "
					+  "WHERE 店所ＣＤ = '" + sKey[0] + "' "
					+    "AND 削除ＦＧ = '0' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				while (reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();
					iCnt++;
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				if(iCnt == 1) 
					sRet[0] = "該当データがありません";
				else
					sRet[0] = "正常終了";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

// ADD 2014.09.10 BEVAS)前田 支店止め対応 START
		/*********************************************************************
		 * 支店止め対応状況の取得
		 * 引数：店所ＣＤ
		 * 戻値：ステータス、
		 * 　　　支店止めＦＧ１("0" →　非対応 / "1" → "対応")
		 * 　　　支店止めＦＧ２("0" →　非対応 / "1" → "対応")
		 *       郵便番号
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_GeneralDelivery(string[] sUser, string[] sKey)
		{
			// DEL 2007.05.10 東都）高木 未使用関数のコメント化
			//			logFileOpen(sUser);
			logWriter(sUser, INF, "店所マスタからの支店止め対応検索開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[4];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				// DEL 2007.05.10 東都）高木 未使用関数のコメント化
				//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
			// DEL 2007.05.10 東都）高木 未使用関数のコメント化
			//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
			//			// 会員チェック
			//			sRet[0] = userCheck2(conn2, sUser);
			//			if(sRet[0].Length > 0)
			//			{
			//				disconnect2(sUser, conn2);
			//				logFileClose();
			//				return sRet;
			//			}
			//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT 支店止めＦＧ１,支店止めＦＧ２,郵便番号 \n"
					+  " FROM ＣＭ１０店所 \n"
					+  " WHERE 店所ＣＤ = '" + sKey[0] + "' \n"
					+  " AND 削除ＦＧ = '0' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
	
				while (reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();
					sRet[2] = reader.GetString(1).Trim();
					sRet[3] = reader.GetString(2).Trim();
					iCnt++;
				}
				// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
				// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				if(iCnt == 1) 
					sRet[0] = "該当データがありません";
				else 
				{
					sRet[0] = "正常終了";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
				// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				// DEL 2007.05.10 東都）高木 未使用関数のコメント化
				//				logFileClose();
			}
			return sRet;
		}
// ADD 2014.09.10 BEVAS)前田 支店止め対応 END

		/*********************************************************************
		 * 店所マスタ一覧取得
		 * 引数：店所ＣＤ、店所名
		 * 戻値：ステータス、店所ＣＤ、店所名
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Get_Shop(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "店所マスタ一覧取得開始");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' "
					+     "|| TRIM(店所ＣＤ) || '|' "
// MOD 2009.11.16 東都）高木 集約店を一覧に追加 START
//					+     "|| TRIM(店所名) || '|' \n"
					+     "|| TRIM(店所名) || '|' "
					+     "|| TRIM(集約店ＣＤ) || '|' "
					+     "\n"
// MOD 2009.11.16 東都）高木 集約店を一覧に追加 END
					+  " FROM ＣＭ１０店所 \n";
				if (sKey[0].Length == 4)
				{
					cmdQuery += " WHERE 店所ＣＤ = '" + sKey[0] + "' \n";
				}
				else
				{
					cmdQuery += " WHERE 店所ＣＤ LIKE '" + sKey[0] + "%' \n";
				}
				if (sKey[1].Length != 0)
				{
					cmdQuery += " AND 店所名 LIKE '" + sKey[1] + "%' \n";
				}
				cmdQuery += " AND 削除ＦＧ = '0' \n"
						 +  " ORDER BY 店所ＣＤ \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "該当データがありません";
				else
				{
					sRet[0] = "正常終了";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

// ADD 2014.09.10 BEVAS)前田 都道府県毎の店所一覧取得 START
		/*********************************************************************
		 * 都道府県毎の店所マスタ一覧取得
		 * 引数：都道府県名
		 * 戻値：ステータス、店所ＣＤ、店所名
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Get_PrefShop(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "店所マスタ一覧取得開始");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' "
					+     "|| TRIM(店所ＣＤ) || '|' "
					+     "|| TRIM(店所名) || '|' "
					+     "|| TRIM(集約店ＣＤ) || '|' "
					+     "|| TRIM(住所) || '|' "
					+     "\n"
					+  " FROM ＣＭ１０店所 \n"
					+  " WHERE 削除ＦＧ = '0' \n";
				if (sKey[0].Length > 0)
				{
					cmdQuery += " AND 住所 LIKE '" + sKey[0] + "%' \n";
				}
				else
				{

				}
				cmdQuery += " ORDER BY 店所ＣＤ \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
				disposeReader(reader);
				reader = null;

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "該当データがありません";
				else
				{
					sRet[0] = "正常終了";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}


// ADD 2014.09.10 BEVAS)前田 都道府県毎の店所一覧取得 END

		/*********************************************************************
		 * 請求先マスタ一覧取得
		 * 引数：店所ＣＤ
		 * 戻値：ステータス、一覧（郵便番号、得意先ＣＤ）...
		 *
		 * 参照元：請求先マスタ.cs 現在未使用
		 *********************************************************************/
		[WebMethod]
		public string[] Get_Claim(string[] sUser, string sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "請求先マスタ一覧取得開始");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' || TRIM(SM04.郵便番号) || '|' "
					+     "|| TRIM(SM04.得意先ＣＤ)     || '|' "
					+     "|| TRIM(SM04.得意先部課ＣＤ) || '|' "
					+     "|| TRIM(SM04.得意先部課名)   || '|' "
					+     "|| TRIM(SM04.会員ＣＤ) || '|' "
					+     "|| NVL(CM01.会員名, ' ')  || '|' "
					+     "|| TO_CHAR(SM04.更新日時) || '|' \n"
					+  " FROM ＣＭ１４郵便番号 CM14 "
					+      ", ＳＭ０４請求先 SM04 \n"
					+  " LEFT JOIN ＣＭ０１会員 CM01 \n"
					+    " ON SM04.会員ＣＤ = CM01.会員ＣＤ "
					+    "AND '0' = CM01.削除ＦＧ \n"
					+ " WHERE CM14.店所ＣＤ = '" + sKey + "' \n"
					+   " AND CM14.郵便番号 = SM04.郵便番号 \n"
// MOD 2010.04.13 東都）高木 郵便番号が削除された時の障害対応 START
//					+   " AND CM14.削除ＦＧ = '0' \n"
// MOD 2010.04.13 東都）高木 郵便番号が削除された時の障害対応 END
					+   " AND SM04.削除ＦＧ = '0' \n"
					+ " ORDER BY SM04.会員ＣＤ "
					+          ",SM04.得意先ＣＤ "
					+          ",SM04.得意先部課ＣＤ \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "該当データがありません";
				else
				{
					sRet[0] = "正常終了";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}
// MOD 2007.01.18 東都）高木 画面レイアウト変更 START
		/*********************************************************************
		 * 請求先マスタ一覧取得２
		 * 引数：店所ＣＤ、会員ＣＤ
		 * 戻値：ステータス、一覧（郵便番号、得意先ＣＤ）...
		 *
		 * 参照元：請求先マスタ.cs
		 *********************************************************************/
		[WebMethod]
		public string[] Get_Claim2(string[] sUser, string sTensyo, string sKaiin)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "請求先マスタ一覧取得２開始");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' || TRIM(SM04.郵便番号) || '|' "
// MOD 2006.12.15 請求先マスタ一覧の変更 START
//					+     "|| TRIM(SM04.得意先ＣＤ)     || '|' "
//					+     "|| TRIM(SM04.得意先部課ＣＤ) || '|' "
//					+     "|| TRIM(SM04.得意先部課名)   || '|' "
//					+     "|| TRIM(SM04.会員ＣＤ) || '|' "
//					+     "|| NVL(CM01.会員名, ' ')  || '|' "
					+     "|| TRIM(SM04.会員ＣＤ) || '|' "
// MOD 2007.01.22 請求先マスタ一覧の変更 START
//					+     "|| NVL(CM01.会員名, ' ')  || '|' "
					+     "|| NVL(TRIM(CM01.会員名), ' ')  || '|' "
// MOD 2007.01.22 請求先マスタ一覧の変更 END
					+     "|| TRIM(SM04.得意先ＣＤ)     || '|' "
					+     "|| TRIM(SM04.得意先部課ＣＤ) || '|' "
					+     "|| TRIM(SM04.得意先部課名)   || '|' "
// MOD 2006.12.15 請求先マスタ一覧の変更 END
					+     "|| TO_CHAR(SM04.更新日時) || '|' \n"
					+  " FROM ＣＭ１４郵便番号 CM14 "
					+      ", ＳＭ０４請求先 SM04 \n"
					+  " LEFT JOIN ＣＭ０１会員 CM01 \n"
					+    " ON SM04.会員ＣＤ = CM01.会員ＣＤ "
					+    "AND '0' = CM01.削除ＦＧ \n"
					+ " WHERE CM14.店所ＣＤ = '" + sTensyo + "' \n";

				if(sKaiin.Length > 0){
					cmdQuery += "AND  SM04.会員ＣＤ = '" + sKaiin + "' \n";
				}
				cmdQuery
					+=  " AND CM14.郵便番号 = SM04.郵便番号 \n"
// MOD 2010.04.13 東都）高木 郵便番号が削除された時の障害対応 START
//					+   " AND CM14.削除ＦＧ = '0' \n"
// MOD 2010.04.13 東都）高木 郵便番号が削除された時の障害対応 END
					+   " AND SM04.削除ＦＧ = '0' \n"
// ADD 2010.12.14 ACT）垣原 王子運送の対応 START
				    +   " AND CM01.管理者区分 IN ('0','1','2') \n"
// ADD 2010.12.14 ACT）垣原 王子運送の対応 END
					+ " ORDER BY SM04.会員ＣＤ "
					+          ",SM04.得意先ＣＤ "
					+          ",SM04.得意先部課ＣＤ \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "該当データがありません";
				else
				{
					sRet[0] = "正常終了";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}
// MOD 2007.01.18 東都）高木 画面レイアウト変更 END

		/*********************************************************************
		 * 請求先マスタ追加
		 * 引数：郵便番号、得意先ＣＤ、得意先部課ＣＤ...
		 * 戻値：ステータス
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Ins_Claim(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "請求先マスタ追加開始");

// MOD 2011.03.09 東都）高木 請求先マスタの主キーに[会員ＣＤ]を追加 START
			string s会員ＣＤ = (sKey.Length > 9) ? sKey[9] : sKey[4];
			if(s会員ＣＤ.Trim().Length == 0) s会員ＣＤ = sKey[4];
// MOD 2011.03.09 東都）高木 請求先マスタの主キーに[会員ＣＤ]を追加 END

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT 削除ＦＧ "
					+   "FROM ＳＭ０４請求先 "
					+  "WHERE 郵便番号 = '" + sKey[0] + "' "
					+    "AND 得意先ＣＤ = '" + sKey[1] + "' "
					+    "AND 得意先部課ＣＤ = '" + sKey[2] + "' "
// MOD 2011.03.09 東都）高木 請求先マスタの主キーに[会員ＣＤ]を追加 START
					+    "AND 会員ＣＤ = '" + s会員ＣＤ + "' \n"
// MOD 2011.03.09 東都）高木 請求先マスタの主キーに[会員ＣＤ]を追加 END
					+    "FOR UPDATE \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				string s削除ＦＧ = "";
				while (reader.Read())
				{
					s削除ＦＧ = reader.GetString(0);
					iCnt++;
				}
				if(iCnt == 1)
				{
					//追加
					cmdQuery
						= "INSERT INTO ＳＭ０４請求先 \n"
						+ " VALUES ('" + sKey[0] + "' " 
						+         ",'" + sKey[1] + "' "
						+         ",'" + sKey[2] + "' "
						+         ",'" + sKey[3] + "' "
						+         ",'" + sKey[4] + "' "
						+         ",'0' "
						+         ",TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
						+         ",'" + sKey[5] + "' "
						+         ",'" + sKey[6] + "' "
						+         ",TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
						+         ",'" + sKey[5] + "' "
						+         ",'" + sKey[6] + "' "
						+ " ) \n";

					CmdUpdate(sUser, conn2, cmdQuery);
					tran.Commit();
					sRet[0] = "正常終了";
				}
				else
				{
					//追加更新
					if (s削除ＦＧ.Equals("1"))
					{
						cmdQuery
							= "UPDATE ＳＭ０４請求先 \n"
							+   " SET 郵便番号 = '" + sKey[0] + "' "
							+       ",得意先部課名 = '" + sKey[3] + "' " 
							+       ",会員ＣＤ = '" + sKey[4] + "' "
							+       ",削除ＦＧ = '0' "
							+       ",登録日時 = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
							+       ",登録ＰＧ = '" + sKey[5] + "' "
							+       ",登録者 = '" + sKey[6] + "' "
							+       ",更新日時 = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
							+       ",更新ＰＧ = '" + sKey[5] + "' "
							+       ",更新者 = '" + sKey[6] + "' \n"
							+ " WHERE 郵便番号 = '" + sKey[0] + "' \n"
							+   " AND 得意先ＣＤ = '" + sKey[1] + "' \n"
							+   " AND 得意先部課ＣＤ = '" + sKey[2] + "' \n"
// MOD 2011.03.09 東都）高木 請求先マスタの主キーに[会員ＣＤ]を追加 START
							+   " AND 会員ＣＤ = '" + s会員ＣＤ + "' \n"
// MOD 2011.03.09 東都）高木 請求先マスタの主キーに[会員ＣＤ]を追加 END
							;

						CmdUpdate(sUser, conn2, cmdQuery);
						tran.Commit();
						sRet[0] = "正常終了";
					}
					else
					{
						tran.Rollback();
						sRet[0] = "既に登録されています";
					}
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 請求先マスタ更新
		 * 引数：郵便番号、得意先ＣＤ、得意先部課ＣＤ...
		 * 戻値：ステータス
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Upd_Claim(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "請求先マスタ更新開始");

// MOD 2011.03.09 東都）高木 請求先マスタの主キーに[会員ＣＤ]を追加 START
			string s会員ＣＤ = (sKey.Length > 9) ? sKey[9] : sKey[4];
// MOD 2011.03.09 東都）高木 請求先マスタの主キーに[会員ＣＤ]を追加 END

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				bool b更新 = true;
				if (!sKey[0].Equals(sKey[7]))
				{
					//郵便番号が変更されていた場合
					cmdQuery
						= "SELECT 削除ＦＧ "
						+   "FROM ＳＭ０４請求先 "
						+  "WHERE 郵便番号 = '" + sKey[0] + "' "
						+    "AND 得意先ＣＤ = '" + sKey[1] + "' "
						+    "AND 得意先部課ＣＤ = '" + sKey[2] + "' "
// MOD 2011.03.09 東都）高木 請求先マスタの主キーに[会員ＣＤ]を追加 START
						+   " AND 会員ＣＤ = '" + s会員ＣＤ + "' \n"
// MOD 2011.03.09 東都）高木 請求先マスタの主キーに[会員ＣＤ]を追加 END
						+    "FOR UPDATE \n";

					OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
					int iCnt = 1;
					string s削除ＦＧ = "";
					while (reader.Read())
					{
						s削除ＦＧ = reader.GetString(0);
						iCnt++;
					}
					if(iCnt == 1) b更新 = true;
					else
					{
						if (s削除ＦＧ.Equals("1"))
						{
							cmdQuery
								= "DELETE FROM ＳＭ０４請求先 "
								+  "WHERE 郵便番号 = '" + sKey[0] + "' "
								+    "AND 得意先ＣＤ = '" + sKey[1] + "' "
								+    "AND 得意先部課ＣＤ = '" + sKey[2] + "' "
// MOD 2011.03.09 東都）高木 請求先マスタの主キーに[会員ＣＤ]を追加 START
								+   " AND 会員ＣＤ = '" + s会員ＣＤ + "' \n"
// MOD 2011.03.09 東都）高木 請求先マスタの主キーに[会員ＣＤ]を追加 END
								+    "AND 削除ＦＧ = '1' ";

							if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
							{
								b更新 = true;
							}
							else
							{
								sRet[0] = "該当データがありません";
								b更新 = false;
							}
						}
						else
						{
							sRet[0] = "既に登録されています";
							b更新 = false;
						}
					}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
					disposeReader(reader);
					reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				}
				if (b更新)
				{
					cmdQuery
						= "UPDATE ＳＭ０４請求先 \n"
						+   " SET 郵便番号 = '" + sKey[0] + "' "
						+       ",得意先部課名 = '" + sKey[3] + "' " 
						+       ",会員ＣＤ = '" + sKey[4] + "' "
						+       ",更新日時 = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
						+       ",更新ＰＧ = '" + sKey[5] + "' "
						+       ",更新者 = '" + sKey[6] + "' \n"
						+ " WHERE 郵便番号 = '" + sKey[7] + "' \n"
						+   " AND 得意先ＣＤ = '" + sKey[1] + "' \n"
						+   " AND 得意先部課ＣＤ = '" + sKey[2] + "' \n"
// MOD 2011.03.09 東都）高木 請求先マスタの主キーに[会員ＣＤ]を追加 START
						+   " AND 会員ＣＤ = '" + s会員ＣＤ + "' \n"
// MOD 2011.03.09 東都）高木 請求先マスタの主キーに[会員ＣＤ]を追加 END
// MOD 2007.02.09 東都）高木 誤りのチェック START
//						+   " AND 更新日時 = '" + sKey[8] + "' \n"
						+   " AND 更新日時 = " + sKey[8] + " \n"
// MOD 2007.02.09 東都）高木 誤りのチェック END
						+   " AND 削除ＦＧ = '0' \n";

					if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
					{
						tran.Commit();
						sRet[0] = "正常終了";
					}
					else
					{
						tran.Rollback();
						sRet[0] = "他の端末で更新されています";
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 請求先マスタ削除
		 * 引数：郵便番号、得意先ＣＤ、得意先部課ＣＤ...
		 * 戻値：ステータス
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Del_Claim(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "請求先マスタ削除開始");
// MOD 2011.03.09 東都）高木 請求先マスタの主キーに[会員ＣＤ]を追加 START
			string s会員ＣＤ = (sKey.Length > 6) ? sKey[6] : "";
			if(s会員ＣＤ.Length == 0){
				return new string[]{"アプリのバージョンが古い為、削除できません。"};
			}
// MOD 2011.03.09 東都）高木 請求先マスタの主キーに[会員ＣＤ]を追加 END
// MOD 2010.08.26 東都）高木 お客様、セクション、請求先削除時のログ強化 START
// MOD 2011.03.09 東都）高木 請求先マスタの主キーに[会員ＣＤ]を追加 START
//			logWriter(sUser, INF, "請求先削除 ["+sKey[0]+"]["+sKey[1]+"]["+sKey[2]+"]");
			logWriter(sUser, INF, "請求先削除 ["+sKey[0]+"]["+sKey[1]+"]["+sKey[2]+"]["+s会員ＣＤ+"]");
// MOD 2011.03.09 東都）高木 請求先マスタの主キーに[会員ＣＤ]を追加 END
// MOD 2010.08.26 東都）高木 お客様、セクション、請求先削除時のログ強化 END

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END
// MOD 2008.11.26 東都）高木 部課コードが空白でもエラーがでなくする START
			if(sKey[2].Length == 0) sKey[2] = " ";
// MOD 2008.11.26 東都）高木 部課コードが空白でもエラーがでなくする END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "UPDATE ＳＭ０４請求先 \n"
					+   " SET 削除ＦＧ = '1' " 
					+       ",更新日時 = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
					+       ",更新ＰＧ = '" + sKey[3] + "' "
					+       ",更新者 = '" + sKey[4] + "' \n"
					+ " WHERE 郵便番号 = '" + sKey[0] + "' \n"
					+   " AND 得意先ＣＤ = '" + sKey[1] + "' \n"
					+   " AND 得意先部課ＣＤ = '" + sKey[2] + "' \n"
// MOD 2011.03.09 東都）高木 請求先マスタの主キーに[会員ＣＤ]を追加 START
					+   " AND 会員ＣＤ = '" + s会員ＣＤ + "' \n"
// MOD 2011.03.09 東都）高木 請求先マスタの主キーに[会員ＣＤ]を追加 END
					+   " AND 更新日時 = " + sKey[5] + " \n";

				if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
					tran.Commit();
					sRet[0] = "正常終了";
				}
				else
				{
					tran.Rollback();
					sRet[0] = "他の端末で更新されています";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 荷送人マスタ取得
		 * 引数：会員ＣＤ、部門ＣＤ、荷送人ＣＤ
		 * 戻値：ステータス
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_Consignor(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "荷送人マスタ検索開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT 荷送人ＣＤ "
					+   "FROM ＳＭ０１荷送人 "
					+  "WHERE 会員ＣＤ = '" + sKey[0] + "' "
// DEL 2008.12.03 東都）高木 荷送人存在チェックから部門ＣＤのしばりをはずす START
//					+    "AND 部門ＣＤ = '" + sKey[1] + "' "
// DEL 2008.12.03 東都）高木 荷送人存在チェックから部門ＣＤのしばりをはずす END
					+    "AND 荷送人ＣＤ = '" + sKey[2] + "' "
					+    "AND 削除ＦＧ = '0' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				while (reader.Read())
				{
					iCnt++;
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				if(iCnt == 1) 
					sRet[0] = "該当データがありません";
				else
					sRet[0] = "正常終了";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 荷送人マスタ一覧取得
		 * 引数：会員ＣＤ、部門ＣＤ、荷送人ＣＤ、カナ
		 * 戻値：ステータス、一覧（名前１、住所１、荷送人ＣＤ）...
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Get_Consignor(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "荷送人マスタ一覧取得開始");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' "
					+     "|| TRIM(名前１) || '|' "
					+     "|| TRIM(住所１) || '|' "
					+     "|| TRIM(荷送人ＣＤ) || '|' "
					+     "|| '(' || TRIM(電話番号１) || ')' "
					+     "|| TRIM(電話番号２) || '-' "
					+     "|| TRIM(電話番号３) || '|' "
					+     "|| TRIM(カナ略称) || '|' \n"
					+  " FROM ＳＭ０１荷送人 \n"
					+ " WHERE 会員ＣＤ = '" + sKey[0] + "' \n"
					+   " AND 部門ＣＤ = '" + sKey[1] + "' \n";
				if (sKey[2].Length == 12)
				{
					cmdQuery += " AND 荷送人ＣＤ = '" + sKey[2] + "' \n";
				}
				else
				{
					cmdQuery += " AND 荷送人ＣＤ LIKE '" + sKey[2] + "%' \n";
				}
				if (sKey[3].Length != 0)
				{
// MOD 2006.06.28 東都）高木 中間一致に修正 START
//					cmdQuery += " AND カナ略称 LIKE '" + sKey[3] + "%' \n";
					cmdQuery += " AND カナ略称 LIKE '%" + sKey[3] + "%' \n";
// MOD 2006.06.28 東都）高木 中間一致に修正 END
				}
				cmdQuery += " AND 削除ＦＧ = '0' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "該当データがありません";
				else
				{
					sRet[0] = "正常終了";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 郵便番号マスタ取得
		 * 引数：郵便番号
		 * 戻値：ステータス、店所名
		 *
		 * 参照元：会員マスタ.cs
		 * 参照元：請求先マスタ.cs
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_Postcode(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "郵便番号マスタ検索開始");

			OracleConnection conn2 = null;
// MOD 2007.01.27 東都）高木 会員申込への追加 START
//			string[] sRet = new string[3];
			string[] sRet = new string[4]{"","","",""};
// MOD 2007.01.27 東都）高木 会員申込への追加 END

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT NVL(CM10.店所名, ' '), \n"
					+ " TRIM(CM14.都道府県名) || TRIM(CM14.市区町村名) || TRIM(CM14.町域名) \n"
// ADD 2007.01.27 東都）高木 会員申込への追加 START
					+ ", CM14.店所ＣＤ \n"
// ADD 2007.01.27 東都）高木 会員申込への追加 END
					+  " FROM ＣＭ１４郵便番号 CM14 \n"
					+  " LEFT JOIN ＣＭ１０店所 CM10 \n"
					+    " ON CM14.店所ＣＤ = CM10.店所ＣＤ "
					+    "AND CM10.削除ＦＧ = '0' \n"
					+ " WHERE CM14.郵便番号 = '" + sKey[0] + "' \n"
//注意 MOD 2010.04.13 東都）高木 郵便番号が削除された時の障害対応 START
//					+   " AND CM14.削除ＦＧ = '0' \n";
//注意 MOD 2010.04.13 東都）高木 郵便番号が削除された時の障害対応 END
					;

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				while (reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();
					sRet[2] = reader.GetString(1).Trim();
// ADD 2007.01.27 東都）高木 会員申込への追加 START
					sRet[3] = reader.GetString(2).Trim();
// ADD 2007.01.27 東都）高木 会員申込への追加 END
					iCnt++;
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				if(iCnt == 1) 
					sRet[0] = "該当データがありません";
				else
					sRet[0] = "正常終了";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 住所マスタ一覧取得(県)
		 * 引数：都道府県ＣＤ
		 * 戻値：ステータス、一覧（市区町村名、市区町村ＣＤ）...
		 *
		 *********************************************************************/
		[WebMethod]
		public String[] Get_byKen(string[] sUser, string s都道府県ＣＤ)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "住所マスタ一覧取得(県)開始");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '||' || TRIM(市区町村名) || '|' "
					+             "|| TRIM(市区町村ＣＤ) || '|' "
					+             "|| '||' \n"
					+  " FROM ＣＭ１２市区町村 \n"
					+ " WHERE 都道府県ＣＤ = '" + s都道府県ＣＤ + "' \n"
					+   " AND 削除ＦＧ = '0' \n"
					+ " ORDER BY 市区町村ＣＤ \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0).Trim());
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "該当データがありません";
				else
				{
					sRet[0] = "正常終了";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 住所マスタ一覧取得(市)
		 * 引数：都道府県ＣＤ、市区町村ＣＤ
		 * 戻値：ステータス、一覧（郵便番号、大字通称名）...
		 *
		 *********************************************************************/
// MOD 2009.06.23 東都）高木 住所マスタのプライマリーキー変更 START
		private static string GET_BYKENSHI_SELECT
			= "SELECT '|' || TRIM(MAX(CM13.郵便番号)) || '|' "
			+ "|| TRIM(MAX(CM13.大字通称名)) || '|' "
			+ "|| TRIM(MAX(CM13.都道府県ＣＤ))"
			+ "|| TRIM(MAX(CM13.市区町村ＣＤ))"
			+ "|| TRIM(MAX(CM13.大字通称ＣＤ)) || '|' "
			+ "|| MIN(NVL(CM10.店所名, ' ')) || '|' \n"
			+  " FROM ＣＭ１３住所 CM13 \n"
			+  " LEFT JOIN ＣＭ１０店所 CM10 \n"
			+    " ON CM13.店所ＣＤ = CM10.店所ＣＤ "
			+    "AND CM10.削除ＦＧ = '0' \n"
			;
		private static string GET_BYKENSHI_WHERE
			= " AND CM13.削除ＦＧ = '0' \n"
			+ " GROUP BY CM13.大字通称ＣＤ \n"
			+ " ORDER BY CM13.大字通称ＣＤ \n"
			;
// MOD 2009.06.23 東都）高木 住所マスタのプライマリーキー変更 END
		[WebMethod]
		public String[] Get_byKenShi(string[] sUser, string s都道府県ＣＤ, string s市区町村ＣＤ)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "住所マスタ一覧取得(市)開始");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			string cmdQuery = "";
			try
			{
				cmdQuery
// MOD 2009.06.23 東都）高木 住所マスタのプライマリーキー変更 START
//					= "SELECT '|' || TRIM(CM13.郵便番号) || '|' "
//					+ "|| TRIM(CM13.大字通称名) || '|' "
//					+ "|| TRIM(CM13.都道府県ＣＤ) || TRIM(CM13.市区町村ＣＤ) || TRIM(CM13.大字通称ＣＤ) || '|' "
//					+ "|| NVL(CM10.店所名, ' ') || '|' \n"
//					+  " FROM ＣＭ１３住所 CM13 \n"
//					+  " LEFT JOIN ＣＭ１０店所 CM10 \n"
//					+    " ON CM13.店所ＣＤ = CM10.店所ＣＤ "
//					+    "AND CM10.削除ＦＧ = '0' \n"
					= GET_BYKENSHI_SELECT
// MOD 2009.06.23 東都）高木 住所マスタのプライマリーキー変更 END
					+ " WHERE CM13.都道府県ＣＤ = '" + s都道府県ＣＤ + "' \n"
					+   " AND CM13.市区町村ＣＤ = '" + s市区町村ＣＤ + "' \n"
// MOD 2009.06.23 東都）高木 住所マスタのプライマリーキー変更 START
//					+   " AND CM13.削除ＦＧ = '0' \n"
//					+ " ORDER BY CM13.大字通称ＣＤ \n";
					+ GET_BYKENSHI_WHERE
					;
// MOD 2009.06.23 東都）高木 住所マスタのプライマリーキー変更 END

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0).Trim());
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "該当データがありません";
				else
				{
					sRet[0] = "正常終了";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 住所マスタ一覧取得
		 * 引数：郵便番号
		 * 戻値：ステータス、一覧（郵便番号、都道府県名）...
		 *
		 * 参照元：住所検索.cs
		 *********************************************************************/
		[WebMethod]
		public String[] Get_byPostcode(string[] sUser, string s郵便番号)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "住所マスタ一覧取得開始");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' || TRIM(CM13.郵便番号) || '|' "
					+ "|| TRIM(CM13.都道府県名) || TRIM(CM13.市区町村名) || TRIM(CM13.大字通称名) || '|' "			//住所
					+ "|| TRIM(CM13.都道府県ＣＤ) || TRIM(CM13.市区町村ＣＤ) || TRIM(CM13.大字通称ＣＤ) || '|' "	//住所ＣＤ
					+ "|| NVL(CM10.店所名, ' ') || '|' \n"
					+  " FROM ＣＭ１３住所 CM13 \n"
					+  " LEFT JOIN ＣＭ１０店所 CM10 \n"
					+    " ON CM13.店所ＣＤ = CM10.店所ＣＤ "
					+    "AND CM10.削除ＦＧ = '0' \n";
				if(s郵便番号.Length == 7)
				{
					cmdQuery += " WHERE CM13.郵便番号 = '" + s郵便番号 + "' \n";
				}
				else
				{
					cmdQuery +=  " WHERE CM13.郵便番号 LIKE '" + s郵便番号 + "%' \n";
				}
				cmdQuery +=    " AND CM13.削除ＦＧ = '0' \n"
					+  " ORDER BY 大字通称ＣＤ \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0).Trim());
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "該当データがありません";
				else
				{
					sRet[0] = "正常終了";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}
		/*********************************************************************
		 * 集約店マスタ取得
		 * 引数：集荷店ＣＤ
		 * 戻値：ステータス、集荷店ＣＤ、店所名...
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_ConnectShop(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "集約店マスタ検索開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[7];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT CM11.集荷店ＣＤ "
					+      ", NVL(CM10S.店所名, ' ') "
					+      ", CM11.集約店ＣＤ "
					+      ", NVL(CM10C.店所名, ' ') "
					+      ", CM11.使用開始日 "
					+      ", CM11.更新日時 \n"
					+  " FROM ＣＭ１１集約店 CM11 \n"
					+  " LEFT JOIN ＣＭ１０店所 CM10S \n"
					+    " ON CM11.集荷店ＣＤ = CM10S.店所ＣＤ "
					+    "AND '0' = CM10S.削除ＦＧ \n"
					+  " LEFT JOIN ＣＭ１０店所 CM10C \n"
					+    " ON CM11.集約店ＣＤ = CM10C.店所ＣＤ "
					+    "AND '0' = CM10C.削除ＦＧ \n"
					+ " WHERE CM11.集荷店ＣＤ = '" + sKey[0] + "' \n"
					+   " AND CM11.削除ＦＧ   = '0' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				while (reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();
					sRet[2] = reader.GetString(1).Trim();
					sRet[3] = reader.GetString(2).Trim();
					sRet[4] = reader.GetString(3).Trim();
					sRet[5] = reader.GetString(4).Trim();
					sRet[6] = reader.GetDecimal(5).ToString().Trim();
					iCnt++;
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				if(iCnt == 1) 
					sRet[0] = "該当データがありません";
				else
					sRet[0] = "正常終了";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 集約店マスタ一覧取得
		 * 引数：集荷店ＣＤ
		 * 戻値：ステータス、一覧（集荷店ＣＤ、店所名）...
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Get_ConnectShop(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "集約店マスタ一覧取得開始");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' "
					+     "|| TRIM(CM11.集荷店ＣＤ) || '|' "
					+     "|| NVL(CM10S.店所名, ' ') || '|' "
					+     "|| '→' || '|' "
					+     "|| TRIM(CM11.集約店ＣＤ) || '|' "
					+     "|| NVL(CM10C.店所名, ' ') || '|' "
					+     "|| TRIM(CM11.使用開始日) || '|' \n"
					+  " FROM ＣＭ１１集約店 CM11 \n"
					+  " LEFT JOIN ＣＭ１０店所 CM10S \n"
					+    " ON CM11.集荷店ＣＤ = CM10S.店所ＣＤ "
					+    "AND '0' = CM10S.削除ＦＧ \n"
					+  " LEFT JOIN ＣＭ１０店所 CM10C \n"
					+    " ON CM11.集約店ＣＤ = CM10C.店所ＣＤ "
					+    "AND '0' = CM10C.削除ＦＧ \n"
					+ " WHERE CM11.集荷店ＣＤ >= '" + sKey[0] + "' \n"
					+   " AND CM11.削除ＦＧ = '0' \n"
					+ " ORDER BY CM11.集荷店ＣＤ \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
				{
					sRet[0] = "該当データがありません";
				}
				else
				{
					sRet[0] = "正常終了";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 集約店マスタ追加
		 * 引数：集荷店ＣＤ、集約店ＣＤ...
		 * 戻値：ステータス
		 *
		 *********************************************************************/
		[WebMethod]
		//集約店マスタ追加
		public string[] Ins_ConnectShop(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "集約店マスタ追加開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];
			string s更新日時 = System.DateTime.Now.ToString("yyyyMMddHHmmss");

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT 削除ＦＧ "
					+   "FROM ＣＭ１１集約店 "
					+  "WHERE 集荷店ＣＤ = '" + sKey[0] + "' "
					+    "FOR UPDATE \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				string s削除ＦＧ = "";
				while (reader.Read())
				{
					s削除ＦＧ = reader.GetString(0);
					iCnt++;
				}
				if(iCnt == 1)
				{
					//追加
					cmdQuery
						= "INSERT INTO ＣＭ１１集約店 \n"
						+ " VALUES ('" + sKey[0] + "' " 
						+         ",'" + sKey[1] + "' "
						+         ",'" + sKey[2] + "' "
						+         ",'0' "
						+         "," + s更新日時
						+         ",'集約登録' "
						+         ",'" + sKey[4] + "' "
						+         "," + s更新日時
						+         ",'集約登録' "
						+         ",'" + sKey[4] + "' \n"
						+ " ) \n";

					CmdUpdate(sUser, conn2, cmdQuery);
					tran.Commit();
					sRet[0] = "正常終了";
				}
				else
				{
					//追加更新
					if (s削除ＦＧ.Equals("1"))
					{
						cmdQuery
							= "UPDATE ＣＭ１１集約店 \n"
							+   " SET 集約店ＣＤ = '" + sKey[1] + "' "
							+       ",使用開始日 = '" + sKey[2] + "' "
							+       ",削除ＦＧ = '0'"
							+       ",登録日時 = " + s更新日時
							+       ",登録ＰＧ = '会員登録'"
							+       ",登録者 = '" + sKey[4] + "' "
							+       ",更新日時 = " + s更新日時
							+       ",更新ＰＧ = '会員登録' "
							+       ",更新者 = '" + sKey[4] + "' \n"
							+ " WHERE 集荷店ＣＤ = '" + sKey[0] + "' \n";

						CmdUpdate(sUser, conn2, cmdQuery);
						tran.Commit();
						sRet[0] = "正常終了";
					}
					else
					{
						tran.Rollback();
						sRet[0] = "既に登録されています";
					}
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 集約店マスタ更新
		 * 引数：集荷店ＣＤ、集約店ＣＤ...
		 * 戻値：ステータス
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Upd_ConnectShop(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "集約店マスタ更新開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];
			string s更新日時 = System.DateTime.Now.ToString("yyyyMMddHHmmss");

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "UPDATE ＣＭ１１集約店 \n"
					+   " SET 集約店ＣＤ = '" + sKey[1] + "' "
					+       ",使用開始日 = '" + sKey[2] + "' " 
					+       ",更新日時 =  " + s更新日時
					+       ",更新ＰＧ = '会員更新' "
					+       ",更新者 = '" + sKey[4] + "' \n"
					+ " WHERE 集荷店ＣＤ = '" + sKey[0] + "' \n"
					+   " AND 削除ＦＧ = '0' \n"
					+   " AND 更新日時 = " + sKey[3] + " \n";

				if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
					tran.Commit();
					sRet[0] = "正常終了";
				}
				else
				{
					tran.Rollback();
					sRet[0] = "他の端末で更新されています";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 集約店マスタ削除
		 * 引数：集荷店ＣＤ、更新日時、更新者
		 * 戻値：ステータス
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Del_ConnectShop(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "集約店マスタ削除開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "UPDATE ＣＭ１１集約店 "
					+    "SET 削除ＦＧ = '1' " 
					+       ",更新日時 = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
					+       ",更新ＰＧ = '集約削除' "
					+       ",更新者 = '" + sKey[2] + "' "
					+  "WHERE 集荷店ＣＤ = '" + sKey[0] + "' "
// MOD 2007.02.09 東都）高木 誤りのチェック START
//					+    "AND 更新日時 = '" + sKey[1] + "' \n";
					+    "AND 更新日時 = " + sKey[1] + " \n";
// MOD 2007.02.09 東都）高木 誤りのチェック END

				if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
					tran.Commit();
					sRet[0] = "正常終了";
				}
				else
				{
					tran.Rollback();
					sRet[0] = "他の端末で更新されています";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * ログイン認証
		 * 引数：会員ＣＤ、利用者ＣＤ、パスワード
		 * 戻値：ステータス、会員ＣＤ、会員名、利用者ＣＤ、利用者名
		 *
		 *********************************************************************/
// ADD 2005.05.11 東都）高木 ORA-03113対策？ START
		private static string SET_LOGIN_SELECT1
// MOD 2006.12.07 東都）小童谷 店所取得削除 START
//			= "SELECT CM01.会員ＣＤ, \n"
//			+ " CM01.会員名, \n"
//			+ " CM04.利用者ＣＤ, \n"
//			+ " CM04.利用者名, \n"
//			+ " CM14.店所ＣＤ  \n"
//			+ " FROM ＣＭ０１会員 CM01, \n"
//			+ " ＣＭ０４利用者 CM04, \n"
//			+ " ＣＭ０２部門 CM02,  \n"
//			+ " ＣＭ１４郵便番号 CM14   \n";

			= "SELECT CM01.会員ＣＤ, \n"
			+ " CM01.会員名, \n"
			+ " CM04.利用者ＣＤ, \n"
			+ " CM04.利用者名 \n"
// ADD 2007.02.06 東都）高木 営業所会員対応 START
			+ ", CM01.管理者区分 \n"
// ADD 2007.02.06 東都）高木 営業所会員対応 END
// ADD 2008.03.21 東都）グローバル対応 START
			+ ", NVL(CM14.店所ＣＤ,' ') \n"
// ADD 2008.03.21 東都）グローバル対応 END
			+ " FROM ＣＭ０１会員 CM01, \n"
// ADD 2008.03.21 東都）グローバル対応 START
			+ " ＣＭ０２部門 CM02, \n"
			+ " ＣＭ１４郵便番号 CM14, \n"
// ADD 2008.03.21 東都）グローバル対応 END
			+ " ＣＭ０４利用者 CM04 \n";
// MOD 2006.12.07 東都）小童谷 店所取得削除 END
// ADD 2005.05.11 東都）高木 ORA-03113対策？ END

		[WebMethod]
		public string[] Set_login(string[] sUser, string[] sKey) 
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "ログイン認証開始");

			OracleConnection conn2 = null;
// MOD 2007.02.06 東都）高木 営業所会員対応 START
//			string[] sRet = new string[5];
// MOD 2008.03.21 東都）グローバル対応 START
//			string[] sRet = new string[6];
			string[] sRet = new string[7];
// MOD 2008.03.21 東都）グローバル対応 END
// MOD 2007.02.06 東都）高木 営業所会員対応 END

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}

			string cmdQuery = "";
			try
			{
// MOD 2005.05.11 東都）高木 ORA-03113対策？ START
//				cmdQuery
//					= "SELECT CM01.会員ＣＤ "
//					+       ",CM01.会員名 "
//					+       ",CM04.利用者ＣＤ "
//					+       ",CM04.利用者名 \n"
//					+  " FROM ＣＭ０１会員 CM01 "
//					+       ",ＣＭ０４利用者 CM04 \n"
				cmdQuery
					= SET_LOGIN_SELECT1
// MOD 2005.05.11 東都）高木 ORA-03113対策？ END
					+ " WHERE CM01.会員ＣＤ = '" + sKey[0] + "' \n"
					+   " AND CM01.会員ＣＤ = CM04.会員ＣＤ \n"
					+   " AND CM04.利用者ＣＤ = '" + sKey[1] + "' \n"
					+   " AND CM04.パスワード = '" + sKey[2] + "' \n"
					+   " AND CM01.使用開始日 <= TO_CHAR(SYSDATE,'YYYYMMDD') \n"
					+   " AND CM01.使用終了日 >= TO_CHAR(SYSDATE,'YYYYMMDD') \n"
// MOD 2005.05.11 東都）高木 ORA-03113対策？ START
//					+   " AND (CM01.管理者区分 = '1' or CM01.管理者区分 = '9') \n"
// MOD 2007.02.06 東都）高木 営業所会員対応 START
//					+   " AND CM01.管理者区分 IN ('1','9') \n"
					+   " AND CM01.管理者区分 IN ('1','2') \n"
// MOD 2007.02.06 東都）高木 営業所会員対応 END
// MOD 2005.05.11 東都）高木 ORA-03113対策？ END
					+   " AND CM01.削除ＦＧ = '0' \n"
					+   " AND CM04.削除ＦＧ = '0' \n"
// ADD 2008.03.21 東都）グローバル対応 START
					+   " AND CM04.会員ＣＤ = CM02.会員ＣＤ \n"
					+   " AND CM04.部門ＣＤ = CM02.部門ＣＤ \n"
					+   " AND           '0' = CM02.削除ＦＧ \n"
					+   " AND CM02.郵便番号 = CM14.郵便番号(+) \n"
// ADD 2008.03.21 東都）グローバル対応 END
					;
// DEL 2006.12.07 東都）小童谷 店所取得削除 START
//					+   " AND CM02.会員ＣＤ = CM04.会員ＣＤ \n"
//					+   " AND CM02.部門ＣＤ = CM04.部門ＣＤ \n"
//					+   " AND CM14.郵便番号 = CM02.郵便番号 \n"
//					+   " AND CM02.削除ＦＧ = '0' \n"
//					+   " AND CM14.削除ＦＧ = '0' \n";
// DEL 2006.12.07 東都）小童谷 店所取得削除 END

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
// MOD 2005.05.11 東都）高木 ORA-03113対策？ START
//				int iCnt = 1;
//				while (reader.Read())
//				{
//					sRet[1] = reader.GetString(0).Trim();
//					sRet[2] = reader.GetString(1).Trim();
//					sRet[3] = reader.GetString(2).Trim();
//					sRet[4] = reader.GetString(3).Trim();
//					iCnt++;
//				}
//				if(iCnt == 1) 
//				{
//					sRet[0] = "該当データがありません";
//				}
//				else
//				{
//					sRet[0] = "正常終了";
//				}
				if (reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();
					sRet[2] = reader.GetString(1).Trim();
					sRet[3] = reader.GetString(2).Trim();
					sRet[4] = reader.GetString(3).Trim();
// MOD 2006.12.07 東都）小童谷 店所取得削除 START
//					sRet[5] = reader.GetString(4).Trim();
// MOD 2006.12.07 東都）小童谷 店所取得削除 END
// ADD 2007.02.06 東都）高木 営業所会員対応 START
					sRet[5] = reader.GetString(4).Trim();
// ADD 2007.02.06 東都）高木 営業所会員対応 END
// ADD 2008.03.21 東都）グローバル対応 START
					sRet[6] = reader.GetString(5).Trim();
// ADD 2008.03.21 東都）グローバル対応 END
					sRet[0] = "正常終了";
				}
				else
				{
					sRet[0] = "該当データがありません";
				}
// MOD 2005.05.11 東都）高木 ORA-03113対策？ END
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 記事データ取得
		 * 引数：会員ＣＤ、部門ＣＤ、記事ＣＤ
		 * 戻値：ステータス、記事ＣＤ、更新日時、状態区分
		 *
		 *********************************************************************/
		[WebMethod]
		public String[] Sel_Kiji(string[] sUser, string sKCode,string sBCode,string sCode)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "記事データ取得開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[4];
			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			try
			{
				string cmdQuery
					= "SELECT 記事, TO_CHAR(更新日時) \n"
					+  " FROM ＳＭ０３記事 \n"
					+ " WHERE 会員ＣＤ = '" + sKCode + "' \n"
					+   " AND 部門ＣＤ = '" + sBCode + "' \n"
					+   " AND 記事ＣＤ = '" + sCode  + "' \n"
					+   " AND 削除ＦＧ = '0' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);

				bool bRead = reader.Read();
				if(bRead == true)
				{
					sRet[1] = reader.GetString(0).TrimEnd();
					sRet[2] = reader.GetString(1);
					sRet[0] = "更新";
					sRet[3] = "U";
				}
				else
				{
					sRet[0] = "登録";
					sRet[3] = "I";
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END

				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			
			return sRet;
		}

		/*********************************************************************
		 * 記事一覧取得
		 * 引数：会員ＣＤ
		 * 戻値：ステータス、一覧（記事ＣＤ、記事、記事種別、更新日時）...
		 *
		 *********************************************************************/
		private static string GET_KIJI_SELECT
			= "SELECT 記事ＣＤ, 記事, 更新日時 \n"
			+  " FROM ＳＭ０３記事 \n";

		private static string GET_KIJI_ORDER
			=   " AND 削除ＦＧ = '0' \n"
			+ " ORDER BY 記事ＣＤ \n";

		[WebMethod]
		public String[] Get_Kiji(string[] sUser, string sKCode, string sBCode)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "記事一覧取得開始");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];
			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			try
			{
				StringBuilder sbQuery = new StringBuilder(512);
				sbQuery.Append(GET_KIJI_SELECT);
				sbQuery.Append(" WHERE 会員ＣＤ = '" + sKCode + "' \n");
				sbQuery.Append(" AND 部門ＣＤ = '" + sBCode + "' \n");
				sbQuery.Append(GET_KIJI_ORDER);

				OracleDataReader reader = CmdSelect(sUser, conn2, sbQuery);

				StringBuilder sbData = new StringBuilder(62); // 4+30+12+4 = 50
				while (reader.Read())
				{
					sbData = new StringBuilder(62);
					sbData.Append("|" + reader.GetString(0).Trim());
					sbData.Append("|" + reader.GetString(1).TrimEnd());
					sbData.Append("|" + reader.GetDecimal(2).ToString().Trim());
					sbData.Append("|");
					sList.Add(sbData);
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "該当データがありません";
				else
				{
					sRet[0] = "正常終了";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}


				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			
			return sRet;
		}

		/*********************************************************************
		 * 記事データ登録
		 * 引数：会員ＣＤ、部門ＣＤ、記事ＣＤ...
		 * 戻値：ステータス
		 *
		 *********************************************************************/
		[WebMethod]
		public String[] Ins_Kiji(string[] sUser, string[] sData)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "記事データ登録開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];
			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			try
			{
				string cmdQuery 
					= "DELETE FROM ＳＭ０３記事 \n"
					+ " WHERE 会員ＣＤ = '" + sData[0] +"' \n"
					+   " AND 部門ＣＤ = '" + sData[1] +"' \n"
					+   " AND 記事ＣＤ = '" + sData[2] +"' \n"
					+   " AND 削除ＦＧ = '1' \n";

				CmdUpdate(sUser, conn2, cmdQuery);

				cmdQuery 
					= "INSERT INTO ＳＭ０３記事  \n"
					+ "VALUES ('" + sData[0] +"','" + sData[1] +"','" + sData[2] +"','" + sData[3] +"', \n"
					+         "'0',TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS'),'" + sData[4] +"','" + sData[5] +"', \n"
					+         "TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS'),'" + sData[4] +"','" + sData[5] +"') \n";

				CmdUpdate(sUser, conn2, cmdQuery);

				tran.Commit();
				sRet[0] = "正常終了";
				
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			
			return sRet;
		}

		/*********************************************************************
		 * 記事データ更新
		 * 引数：会員ＣＤ、部門ＣＤ、記事ＣＤ...
		 * 戻値：ステータス
		 *
		 *********************************************************************/
		[WebMethod]
		public String[] Upd_Kiji(string[] sUser, string[] sData)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "記事データ更新開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];
			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			try
			{
				string cmdQuery 
					= "UPDATE ＳＭ０３記事 \n"
					+   " SET 記事     = '" + sData[3] +"', \n"
					+       " 削除ＦＧ = '0', \n"
					+       " 更新ＰＧ = '" + sData[4] +"', \n"
					+       " 更新者   = '" + sData[5] +"', \n"
					+       " 更新日時 =  TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS')  \n"
					+ " WHERE 会員ＣＤ = '" + sData[0] +"' \n"
					+   " AND 部門ＣＤ = '" + sData[1] +"' \n"
					+   " AND 記事ＣＤ = '" + sData[2] +"' \n"
					+   " AND 更新日時 =  " + sData[6] +" \n";

				int iUpdRow = CmdUpdate(sUser, conn2, cmdQuery);

				tran.Commit();
				if(iUpdRow == 0)
					sRet[0] = "データ編集中に他の端末より更新されています。\r\n再度、最新データを呼び出して更新してください。";
				else				
					sRet[0] = "正常終了";

				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			
			return sRet;
		}

		/*********************************************************************
		 * 記事データ削除
		 * 引数：会員ＣＤ、部門ＣＤ、記事ＣＤ、更新ＰＧ、更新者
		 * 戻値：ステータス
		 *
		 *********************************************************************/
		[WebMethod]
		public String[] Del_Kiji(string[] sUser, string[] sData)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "記事データ削除開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];
			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			try
			{
				StringBuilder sbQuery = new StringBuilder(1024);
				sbQuery.Append
					( "UPDATE ＳＭ０３記事 \n"
					+   " SET 削除ＦＧ = '1', \n"
					+       " 更新ＰＧ = '" + sData[3] +"', \n"
					+       " 更新者   = '" + sData[4] +"', \n"
					+       " 更新日時 =  TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') \n"
					+ " WHERE 会員ＣＤ = '" + sData[0] +"' \n"
					+   " AND 部門ＣＤ = '" + sData[1] +"' \n"
					+   " AND 記事ＣＤ = '" + sData[2] +"' \n");

				CmdUpdate(sUser, conn2, sbQuery);

				tran.Commit();				
				sRet[0] = "正常終了";

				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			
			return sRet;
		}

		/*********************************************************************
		 * 会員情報取得（ＣＳＶ出力用）
		 * 引数：会員ＣＤ、使用開始日（開始、終了）、使用終了日（開始、終了）、
		 *		 利用者登録日（開始、終了）
		 * 戻値：ステータス、会員ＣＤ、会員名、使用開始日...
		 *
		 * 参照元：会員情報ＣＳＶ出力.cs
		 *********************************************************************/
		private static string GET_KAIINCSV_SELECT
			= "SELECT R.会員ＣＤ,NVL(K.会員名,' '),NVL(K.使用開始日,' '),NVL(K.使用終了日,' '), \n"
			+       " R.部門ＣＤ,NVL(B.部門名,' '),NVL(Y.店所ＣＤ,' '),NVL(T.店所名,' '), \n"
			+       " NVL(B.設置先住所１,' '),NVL(B.設置先住所２,' '), \n"
			+       " R.利用者ＣＤ,R.\"パスワード\",R.利用者名,SUBSTR(R.登録日時,1,8) \n"
// ADD 2006.12.11 東都）小童谷 サーマル台数追加 START
//			+       " ,NVL(B.サーマル台数,'0')\n"
			+       " ,NVL(B.\"サーマル台数\",'0')\n"
// ADD 2006.12.11 東都）小童谷 サーマル台数追加 END
// MOD 2009.03.03 東都）高木 サーマルシリアル番号の追加 START
//			+      ", NVL(B.シリアル番号１,' '), NVL(B.状態１,' ') \n"
//			+      ", NVL(B.シリアル番号２,' '), NVL(B.状態２,' ') \n"
//			+      ", NVL(B.シリアル番号３,' '), NVL(B.状態３,' ') \n"
//			+      ", NVL(B.シリアル番号４,' '), NVL(B.状態４,' ') \n"
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） START
//			+      ", NVL(B.シリアル番号１,' '), DECODE(B.状態１,'1 ','返品','2 ','不良品','3 ','不明','4 ','その他','5 ','発送中',' ') \n"
//			+      ", NVL(B.シリアル番号２,' '), DECODE(B.状態２,'1 ','返品','2 ','不良品','3 ','不明','4 ','その他','5 ','発送中',' ') \n"
//			+      ", NVL(B.シリアル番号３,' '), DECODE(B.状態３,'1 ','返品','2 ','不良品','3 ','不明','4 ','その他','5 ','発送中',' ') \n"
//			+      ", NVL(B.シリアル番号４,' '), DECODE(B.状態４,'1 ','返品','2 ','不良品','3 ','不明','4 ','その他','5 ','発送中',' ') \n"
			+      ", NVL(CM06.シリアル番号１,' '), DECODE(CM06.状態１,'1 ','返品','2 ','不良品','3 ','不明','4 ','その他','5 ','発送中',' ') \n"
			+      ", NVL(CM06.シリアル番号２,' '), DECODE(CM06.状態２,'1 ','返品','2 ','不良品','3 ','不明','4 ','その他','5 ','発送中',' ') \n"
			+      ", NVL(CM06.シリアル番号３,' '), DECODE(CM06.状態３,'1 ','返品','2 ','不良品','3 ','不明','4 ','その他','5 ','発送中',' ') \n"
			+      ", NVL(CM06.シリアル番号４,' '), DECODE(CM06.状態４,'1 ','返品','2 ','不良品','3 ','不明','4 ','その他','5 ','発送中',' ') \n"
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） END
// MOD 2009.11.25 東都）高木 お客様情報出力（ＣＳＶ）の項目追加 START
			+      ", DECODE(K.管理者区分,'0','一般','1','管理者','2','営業所', K.管理者区分) \n"
			+      ", DECODE(K.記事連携ＦＧ,'0',' ','1','運賃非表示', K.記事連携ＦＧ) \n"
			+      ", K.登録日時, K.更新日時 \n"
			+      ", B.組織ＣＤ, B.郵便番号, NVL(CM06.使用料,0) \n"
			+      ", DECODE(CM06.会員申込管理番号,NULL,' ',0,' ',TO_CHAR(CM06.会員申込管理番号)) \n"
			+      ", B.登録日時, B.更新日時 \n"
			+      ", R.荷送人ＣＤ \n"
			+      ", DECODE(R.権限１,' ',' ','1','ラベル印刷禁止', R.権限１) \n"
			+      ", R.\"認証エラー回数\" \n"
			+      ", R.登録ＰＧ \n"
			+      ", R.登録日時, R.更新日時 \n"
// MOD 2009.11.25 東都）高木 お客様情報出力（ＣＳＶ）の項目追加 END
// MOD 2009.03.03 東都）高木 サーマルシリアル番号の追加 END
			+ " FROM ＣＭ０１会員 K,ＣＭ０２部門 B,ＣＭ０４利用者 R,ＣＭ１０店所 T,ＣＭ１４郵便番号 Y \n"
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） START
			+ " ,ＣＭ０６部門拡張 CM06 \n"
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） EMD
			;

		[WebMethod]
		public String[] Get_csvwrite(string[] sUser, string[] sData)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "会員情報ＣＳＶ出力用取得開始");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();

			string[] sRet = new string[1];
			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 START
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//// ADD 2005.05.23 東都）伊賀 会員チェック追加 END

			StringBuilder sbQuery = new StringBuilder(1024);
			StringBuilder sbQuery2 = new StringBuilder(1024);
			try
			{
// MOD 2005.07.13 東都）高木 条件の変更 START
//				sbQuery.Append(" WHERE R.会員ＣＤ = K.会員ＣＤ(+) \n");
//				sbQuery.Append(" AND R.会員ＣＤ = B.会員ＣＤ(+) \n");
//				sbQuery.Append(" AND R.部門ＣＤ = B.部門ＣＤ(+) \n");
				sbQuery.Append(" WHERE R.会員ＣＤ = K.会員ＣＤ \n");
				sbQuery.Append(" AND R.会員ＣＤ = B.会員ＣＤ \n");
				sbQuery.Append(" AND R.部門ＣＤ = B.部門ＣＤ \n");
// MOD 2005.07.13 東都）高木 条件の変更 END
				sbQuery.Append(" AND B.郵便番号 = Y.郵便番号(+) \n");
				sbQuery.Append(" AND Y.店所ＣＤ = T.店所ＣＤ(+) \n");
				sbQuery.Append(" AND R.削除ＦＧ = '0' \n");
// MOD 2005.07.13 東都）高木 条件の変更 START
//				sbQuery.Append(" AND '0' = B.削除ＦＧ(+) \n");
				sbQuery.Append(" AND '0' = K.削除ＦＧ \n");
				sbQuery.Append(" AND '0' = B.削除ＦＧ \n");
// MOD 2005.07.13 東都）高木 条件の変更 END
				sbQuery.Append(" AND '0' = T.削除ＦＧ(+) \n");
//注意 MOD 2010.04.13 東都）高木 郵便番号が削除された時の障害対応 START
//				sbQuery.Append(" AND '0' = Y.削除ＦＧ(+) \n");
//注意 MOD 2010.04.13 東都）高木 郵便番号が削除された時の障害対応 END
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） START
				sbQuery.Append(" AND R.会員ＣＤ = CM06.会員ＣＤ(+) \n");
				sbQuery.Append(" AND R.部門ＣＤ = CM06.部門ＣＤ(+) \n");
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） END
// MOD 2011.05.31 東都）高木 王子運送の対応 START
				sbQuery.Append(" AND K.管理者区分 IN ('0','1','2') \n"); // 0:一般 1:管理者 2:営業所
// MOD 2011.05.31 東都）高木 王子運送の対応 END

				if(sData[0].Length > 0 && sData[1].Length > 0)
					sbQuery.Append(" AND R.会員ＣＤ  BETWEEN '"+ sData[0] + "' AND '"+ sData[1] +"' \n");
				else
				{
					if(sData[0].Length > 0 && sData[1].Length == 0)
						sbQuery.Append(" AND R.会員ＣＤ = '"+ sData[0] + "' \n");
				}

				if(sData[2].Length > 0 && sData[3].Length > 0)
					sbQuery.Append(" AND K.使用開始日  BETWEEN '"+ sData[2] + "' AND '"+ sData[3] +"' \n");
				else
				{
					if(sData[2].Length > 0 && sData[3].Length == 0)
						sbQuery.Append(" AND K.使用開始日 = '"+ sData[2] + "' \n");
				}

				if(sData[4].Length > 0 && sData[5].Length > 0)
					sbQuery.Append(" AND K.使用終了日  BETWEEN '"+ sData[4] + "' AND '"+ sData[5] +"' \n");
				else
				{
					if(sData[4].Length > 0 && sData[5].Length == 0)
						sbQuery.Append(" AND K.使用終了日 = '"+ sData[4] + "' \n");
				}

				if(sData[6].Length > 0 && sData[7].Length > 0)
					sbQuery.Append(" AND SUBSTR(R.登録日時,1,8)  BETWEEN '"+ sData[6] + "' AND '"+ sData[7] +"' \n");
				else
				{
					if(sData[6].Length > 0 && sData[7].Length == 0)
						sbQuery.Append(" AND SUBSTR(R.登録日時,1,8) = '"+ sData[6] + "' \n");
				}
				sbQuery.Append(" ORDER BY R.会員ＣＤ,R.利用者ＣＤ ");


				OracleDataReader reader;

				sbQuery2.Append(GET_KAIINCSV_SELECT);
				sbQuery2.Append(sbQuery);
				reader = CmdSelect(sUser, conn2, sbQuery2);

				StringBuilder sbData = new StringBuilder(1024);
				while (reader.Read())
				{
					sbData = new StringBuilder(1024);
					sbData.Append(sDbl + sSng + reader.GetString(0).Trim() + sDbl);				// 会員ＣＤ
					sbData.Append(sKanma + sDbl + reader.GetString(1).Trim() + sDbl);			// 会員名
					sbData.Append(sKanma + sDbl + reader.GetString(2).Trim() + sDbl);			// 使用開始日
					sbData.Append(sKanma + sDbl + reader.GetString(3).Trim() + sDbl);			// 使用終了日
// MOD 2009.11.25 東都）高木 お客様情報出力（ＣＳＶ）の項目追加 START
					sbData.Append(sKanma + sDbl + reader.GetString(23).TrimEnd() + sDbl);		// 管理者区分
					sbData.Append(sKanma + sDbl + reader.GetString(24).TrimEnd() + sDbl);		// 運賃非表示（記事連携ＦＧ）
					sbData.Append(sKanma + sDbl + sSng + reader.GetDecimal(25).ToString().TrimEnd() + sDbl); // 登録日時
					sbData.Append(sKanma + sDbl + sSng + reader.GetDecimal(26).ToString().TrimEnd() + sDbl); // 更新日時
// MOD 2009.11.25 東都）高木 お客様情報出力（ＣＳＶ）の項目追加 END
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(4).Trim() + sDbl);	// 部門ＣＤ
					sbData.Append(sKanma + sDbl + reader.GetString(5).Trim() + sDbl);			// 部門名
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(6).Trim() + sDbl);	// 管理店所ＣＤ
					sbData.Append(sKanma + sDbl + reader.GetString(7).Trim() + sDbl);			// 管理店所名
// MOD 2009.11.25 東都）高木 お客様情報出力（ＣＳＶ）の項目追加 START
//					sbData.Append(sKanma + sDbl + reader.GetString(8).Trim() + sDbl);			// 設置先住所１
//					sbData.Append(sKanma + sDbl + reader.GetString(9).Trim() + sDbl);			// 設置先住所２
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(8).Trim() + sDbl);	// 設置先住所１
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(9).Trim() + sDbl);	// 設置先住所２
// MOD 2009.11.25 東都）高木 お客様情報出力（ＣＳＶ）の項目追加 END
// MOD 2009.11.25 東都）高木 お客様情報出力（ＣＳＶ）の項目追加 START
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(27).TrimEnd() + sDbl);		// Ver.（組織ＣＤ）
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(28).TrimEnd() + sDbl);		// 郵便番号
					sbData.Append(sKanma + sDbl + reader.GetDecimal(29).ToString().TrimEnd() + sDbl); // 使用料
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(30).TrimEnd() + sDbl); // 会員申込管理番号
					sbData.Append(sKanma + sDbl + sSng + reader.GetDecimal(31).ToString().TrimEnd() + sDbl); // 登録日時
					sbData.Append(sKanma + sDbl + sSng + reader.GetDecimal(32).ToString().TrimEnd() + sDbl); // 更新日時
// MOD 2009.11.25 東都）高木 お客様情報出力（ＣＳＶ）の項目追加 END
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(10).Trim() + sDbl);	// 利用者ＣＤ
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(11).Trim() + sDbl);	// パスワード
					sbData.Append(sKanma + sDbl + reader.GetString(12).Trim() + sDbl       );	// 利用者名
					sbData.Append(sKanma + sDbl + reader.GetString(13).Trim() + sDbl);			// 利用者登録日
// MOD 2009.11.25 東都）高木 お客様情報出力（ＣＳＶ）の項目追加 START
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(33).TrimEnd() + sDbl); // 荷送人ＣＤ
					sbData.Append(sKanma + sDbl + reader.GetString(34).TrimEnd() + sDbl);		 // ラベル印刷禁止
					sbData.Append(sKanma + sDbl + reader.GetDecimal(35).ToString().TrimEnd() + sDbl); // 認証エラー回数
					sbData.Append(sKanma + sDbl + reader.GetString(36).TrimEnd() + sDbl); // パスワード更新日（登録ＰＧ）
					sbData.Append(sKanma + sDbl + sSng + reader.GetDecimal(37).ToString().TrimEnd() + sDbl); // 登録日時
					sbData.Append(sKanma + sDbl + sSng + reader.GetDecimal(38).ToString().TrimEnd() + sDbl); // 更新日時
// MOD 2009.11.25 東都）高木 お客様情報出力（ＣＳＶ）の項目追加 END
// ADD 2006.12.11 東都）小童谷 サーマル台数追加 START
					sbData.Append(sKanma + sDbl + reader.GetDecimal(14) + sDbl);			// サーマル台数
// ADD 2006.12.11 東都）小童谷 サーマル台数追加 END
// ADD 2009.03.03 東都）高木 サーマルシリアル番号の追加 START
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(15).Trim() + sDbl);	// シリアル番号１
					sbData.Append(sKanma + sDbl + reader.GetString(16).Trim() + sDbl);			// 状態１
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(17).Trim() + sDbl);	// シリアル番号２
					sbData.Append(sKanma + sDbl + reader.GetString(18).Trim() + sDbl);			// 状態２
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(19).Trim() + sDbl);	// シリアル番号３
					sbData.Append(sKanma + sDbl + reader.GetString(20).Trim() + sDbl);			// 状態３
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(21).Trim() + sDbl);	// シリアル番号４
					sbData.Append(sKanma + sDbl + reader.GetString(22).Trim() + sDbl);			// 状態４
// ADD 2009.03.03 東都）高木 サーマルシリアル番号の追加 END

					sList.Add(sbData);
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "該当データがありません";
				else
				{
					sRet[0] = "正常終了";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}


// ADD 2005.05.27 東都）西口 メッセージ登録画面追加 START
		/*********************************************************************
		 * システム管理データ取得
		 * 引数：会員ＣＤ、システム管理ＣＤ
		 * 戻値：ステータス、メッセージ、更新日時
		 *
		 *********************************************************************/
		[WebMethod]
		public String[] Sel_Syskanri(string[] sUser, string sKanCode)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "システム管理データ取得開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[4];
			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}

			try
			{
				string cmdQuery
					= "SELECT \"メッセージ\", TO_CHAR(更新日時) \n"
					+  " FROM ＡＭ０１システム管理 \n"
					+ " WHERE システム管理ＣＤ = '" + sKanCode + "' \n"
					+   " AND 削除ＦＧ = '0' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);

				bool bRead = reader.Read();
				if(bRead == true)
				{
					sRet[1] = reader.GetString(0).TrimEnd();
					sRet[2] = reader.GetString(1);
					sRet[0] = "正常終了";
				}
				else
				{
					sRet[0] = "該当データがありません";
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END

				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			
			return sRet;
		}

		/*********************************************************************
		 * 記事データ更新
		 * 引数：会員ＣＤ、システム管理ＣＤ、メッセージ
		 * 戻値：ステータス
		 *
		 *********************************************************************/
		[WebMethod]
		public String[] Upd_Syskanri(string[] sUser, string[] sData)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "システム管理データ更新開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];
			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			try
			{
				string cmdQuery 
					= "UPDATE ＡＭ０１システム管理 \n"
					+   " SET \"メッセージ\" = '" + sData[1] +"', \n"
					+       " 削除ＦＧ   = '0', \n"
					+       " 更新ＰＧ   = '" + sData[2] +"', \n"
					+       " 更新者     = '" + sData[3] +"', \n"
					+       " 更新日時   =  TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS')  \n"
					+ " WHERE システム管理ＣＤ = '" + sData[0] +"' \n"
					+   " AND 更新日時 =  " + sData[4] +" \n";

				int iUpdRow = CmdUpdate(sUser, conn2, cmdQuery);

				tran.Commit();
				if(iUpdRow == 0)
					sRet[0] = "データ編集中に他の端末より更新されています。\r\n再度、最新データを呼び出して更新してください。";
				else				
					sRet[0] = "正常終了";

				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			
			return sRet;
		}
// ADD 2005.05.27 東都）西口 メッセージ登録画面追加 END
// ADD 2006.08.03 東都）高木 会員申込機能の追加 START
		/*********************************************************************
		 * 申込情報取得
		 * 引数：管理番号
		 * 戻値：ステータス、管理番号、会員名、使用開始日、管理者区分、使用終了日
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_Mosikomi(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "申込情報検索開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[43];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT 管理番号 "
					+       ",店所ＣＤ "
					+       ",申込者カナ "
					+       ",申込者名 "
					+       ",申込者郵便番号 \n"
					+       ",申込者県ＣＤ "
					+       ",申込者住所１ "
					+       ",申込者住所２ "
					+       ",申込者電話１ "
					+       ",申込者電話２ \n"
					+       ",申込者電話３ "
					+       ",申込者電話 "
					+       ",申込者ＦＡＸ１ "
					+       ",申込者ＦＡＸ２ "
					+       ",申込者ＦＡＸ３ \n"
					+       ",設置場所区分 "
					+       ",設置場所カナ "
					+       ",設置場所名 "
					+       ",設置場所郵便番号 "
					+       ",設置場所県ＣＤ \n"
					+       ",設置場所住所１ "
					+       ",設置場所住所２ "
					+       ",設置場所電話１ "
					+       ",設置場所電話２ "
					+       ",設置場所電話３ \n"
					+       ",設置場所ＦＡＸ１ "
					+       ",設置場所ＦＡＸ２ "
					+       ",設置場所ＦＡＸ３ "
					+       ",設置場所担当者名 "
					+       ",設置場所役職名 \n"
					+       ",設置場所使用料 "
					+       ",会員ＣＤ "
					+       ",使用開始日 "
					+       ",部門ＣＤ "
					+       ",部門名 \n"
					+       ",\"サーマル台数\" "
					+       ",利用者ＣＤ "
					+       ",利用者名 "
					+       ",\"パスワード\" "
					+       ",承認状態ＦＧ \n"
					+       ",メモ "
					+       ",TO_CHAR(更新日時) "
					+       ",更新者 \n"
					+  " FROM ＳＭ０５会員申込 \n"
					+ " WHERE 管理番号 = '" + sKey[0] + "' \n"
					+    "AND 削除ＦＧ = '0' \n";

				if(sKey[1].Trim().Length !=0)
					cmdQuery += "AND 店所ＣＤ = '" + sKey[1] + "' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				if(reader.Read())
				{
					sRet[0] = "正常終了";
					//管理番号はダミー
					sRet[1] = reader.GetString(1).Trim();
					sRet[2] = reader.GetString(2).Trim();
					sRet[3] = reader.GetString(3).Trim();
					sRet[4] = reader.GetString(4).Trim();
					sRet[5] = reader.GetString(5).Trim();
					sRet[6] = reader.GetString(6).Trim();
					sRet[7] = reader.GetString(7).Trim();
					sRet[8] = reader.GetString(8).Trim();
					sRet[9] = reader.GetString(9).Trim();
					sRet[10] = reader.GetString(10).Trim();
					sRet[11] = reader.GetString(11).Trim();
					sRet[12] = reader.GetString(12).Trim();
					sRet[13] = reader.GetString(13).Trim();
					sRet[14] = reader.GetString(14).Trim();
					sRet[15] = reader.GetString(15).Trim();
					sRet[16] = reader.GetString(16).Trim();
					sRet[17] = reader.GetString(17).Trim();
					sRet[18] = reader.GetString(18).Trim();
					sRet[19] = reader.GetString(19).Trim();
					sRet[20] = reader.GetString(20).Trim();
					sRet[21] = reader.GetString(21).Trim();
					sRet[22] = reader.GetString(22).Trim();
					sRet[23] = reader.GetString(23).Trim();
					sRet[24] = reader.GetString(24).Trim();
					sRet[25] = reader.GetString(25).Trim();
					sRet[26] = reader.GetString(26).Trim();
					sRet[27] = reader.GetString(27).Trim();
					sRet[28] = reader.GetString(28).Trim();
					sRet[29] = reader.GetString(29).Trim();
					sRet[30] = reader.GetDecimal(30).ToString().Trim();
					sRet[31] = reader.GetString(31).Trim();
					sRet[32] = reader.GetString(32).Trim();
					sRet[33] = reader.GetString(33).Trim();
					sRet[34] = reader.GetString(34).Trim();
					sRet[35] = reader.GetDecimal(35).ToString().Trim();
					sRet[36] = reader.GetString(36).Trim();
					sRet[37] = reader.GetString(37).Trim();
					sRet[38] = reader.GetString(38).Trim();
					sRet[39] = reader.GetString(39).Trim();
					sRet[40] = reader.GetString(40).Trim();
					sRet[41] = reader.GetString(41).Trim();
					sRet[42] = reader.GetString(42).Trim();
				}
				else
				{
					sRet[0] = "該当データがありません";
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 申込情報一覧取得
		 * 引数：管理番号、会員名
		 * 戻値：ステータス、管理番号、会員名
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Get_Mosikomi(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "申込情報一覧取得開始");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' "
					+     "|| TO_CHAR(管理番号,'0000000') || '|' "
					+     "|| TRIM(申込者名) || '|' "
					+     "|| TRIM(申込者カナ) || '|' "
					+     "|| TRIM(店所ＣＤ) || '|' \n"
					+  " FROM ＳＭ０５会員申込 \n";

				bool bWhere = true;
				if (sKey[0].Trim().Length != 0)
				{
					if(bWhere){ cmdQuery+=" WHERE"; bWhere = false;} else cmdQuery+=" AND";
					cmdQuery += " 管理番号 = " + sKey[0] + " \n";
				}
				if (sKey[1].Trim().Length != 0)
				{
					if(bWhere){ cmdQuery+=" WHERE"; bWhere = false;} else cmdQuery+=" AND";
					cmdQuery += " 店所ＣＤ = '" + sKey[1] + "' \n";
				}
				if (sKey[2].Trim().Length != 0)
				{
					if(bWhere){ cmdQuery+=" WHERE"; bWhere = false;} else cmdQuery+=" AND";
					cmdQuery += " 申込者名 LIKE '%" + sKey[2] + "%' \n";
				}
				if (sKey[3].Trim().Length != 0)
				{
					if(bWhere){ cmdQuery+=" WHERE"; bWhere = false;} else cmdQuery+=" AND";
					cmdQuery += " 申込者カナ LIKE '%" + sKey[3] + "%' \n";
				}
				if(bWhere){ cmdQuery+=" WHERE "; bWhere = false;} else cmdQuery+=" AND";
				cmdQuery += " 削除ＦＧ = '0' \n";
				cmdQuery += " ORDER BY 管理番号 \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);

				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0)
				{
					sRet[0] = "該当データがありません";
				}
				else
				{
					sRet[0] = "正常終了";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 申込情報追加
		 * 引数：管理番号、会員名...
		 * 戻値：ステータス、更新日時、管理番号
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Ins_Mosikomi(string[] sUser, string[] sData)
		{
			//管理番号の取得
			string[] sKey   = {" ", sData[42]};	//店所ＣＤ、更新者
			string[] sKanri = Get_KaniSaiban(sUser, sKey);
			if(sKanri[0].Length > 4)
			{
				return sKanri;
			}
			sData[0] = sKanri[1];

// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "申込情報追加開始");

			OracleConnection conn2 = null;

//			string[] sRet = new string[3]{"","",""};
//			string s更新日時 = System.DateTime.Now.ToString("yyyyMMddHHmmss");
			string s更新日時 = System.DateTime.Now.ToString("yyyyMMddHHmmss");
			string[] sRet = new string[3]{"", s更新日時, sData[0]};

// ADD 2007.01.27 東都）高木 会員申込への追加 START
			string s更新ＰＧ = "申込登録";
			if(sData.Length > 43)
				s更新ＰＧ = sData[43];
// ADD 2007.01.27 東都）高木 会員申込への追加 END

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT 削除ＦＧ \n"
					+   "FROM ＳＭ０５会員申込 \n"
					+  "WHERE 管理番号 = " + sData[0] + " \n"
					+    "FOR UPDATE \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				string s削除ＦＧ = "";
				if(reader.Read())
				{
					s削除ＦＧ = reader.GetString(0);
					iCnt++;
				}

				if(iCnt == 1)
				{
					//追加
					cmdQuery
						= "INSERT INTO ＳＭ０５会員申込 \n"
						+ " VALUES ( " + sData[0] + "  " 
						+         ",'" + sData[1] + "' "
						+         ",'" + sData[2] + "' "
						+         ",'" + sData[3] + "' "
						+         ",'" + sData[4] + "' \n"
						+         ",'" + sData[5] + "' "
						+         ",'" + sData[6] + "' "
						+         ",'" + sData[7] + "' "
						+         ",'" + sData[8] + "' "
						+         ",'" + sData[9] + "' \n"
						+         ",'" + sData[10] + "' "
						+         ",'" + sData[11] + "' "
						+         ",'" + sData[12] + "' "
						+         ",'" + sData[13] + "' "
						+         ",'" + sData[14] + "' \n"
						+         ",'" + sData[15] + "' "
						+         ",'" + sData[16] + "' "
						+         ",'" + sData[17] + "' "
						+         ",'" + sData[18] + "' "
						+         ",'" + sData[19] + "' \n"
						+         ",'" + sData[20] + "' "
						+         ",'" + sData[21] + "' "
						+         ",'" + sData[22] + "' "
						+         ",'" + sData[23] + "' "
						+         ",'" + sData[24] + "' \n"
						+         ",'" + sData[25] + "' "
						+         ",'" + sData[26] + "' "
						+         ",'" + sData[27] + "' "
						+         ",'" + sData[28] + "' "
						+         ",'" + sData[29] + "' \n"
						+         ", " + sData[30] + "  "
						+         ",'" + sData[31] + "' "
						+         ",'" + sData[32] + "' "
						+         ",'" + sData[33] + "' "
						+         ",'" + sData[34] + "' \n"
						+         ", " + sData[35] + "  "
						+         ",'" + sData[36] + "' "
						+         ",'" + sData[37] + "' "
						+         ",'" + sData[38] + "' "
						+         ",'" + sData[39] + "' \n"
						+         ",'" + sData[40] + "' \n"
						+         ",'0' \n"
						+         "," + s更新日時
// MOD 2007.01.27 東都）高木 会員申込への追加 START
//						+         ",'申込登録' "
						+         ",'" + s更新ＰＧ + "' "
// MOD 2007.01.27 東都）高木 会員申込への追加 END
						+         ",'" + sData[42] + "' \n"
						+         "," + s更新日時
// MOD 2007.01.27 東都）高木 会員申込への追加 START
//						+         ",'申込登録' "
						+         ",'" + s更新ＰＧ + "' "
// MOD 2007.01.27 東都）高木 会員申込への追加 END
						+         ",'" + sData[42] + "' \n"
						+ " ) \n";

					CmdUpdate(sUser, conn2, cmdQuery);

					tran.Commit();
					sRet[0] = "正常終了";

//					sRet[1] = s更新日時;
//					sRet[2] = sData[0];		//管理番号

				}
				else
				{
					//追加更新
					if (s削除ＦＧ.Equals("1"))
					{
						cmdQuery
							= "UPDATE ＳＭ０５会員申込 \n"
							+   " SET 店所ＣＤ = '" + sData[1] + "' \n"
							+       ",申込者カナ = '" + sData[2] + "' \n"
							+       ",申込者名 = '" + sData[3] + "' \n"
							+       ",申込者郵便番号 = '" + sData[4] + "' \n"
							+       ",申込者県ＣＤ = '" + sData[5] + "' \n"
							+       ",申込者住所１ = '" + sData[6] + "' \n"
							+       ",申込者住所２ = '" + sData[7] + "' \n"
							+       ",申込者電話１ = '" + sData[8] + "' \n"
							+       ",申込者電話２ = '" + sData[9] + "' \n"
							+       ",申込者電話３ = '" + sData[10] + "' \n"
							+       ",申込者電話 = '" + sData[11] + "' \n"
							+       ",申込者ＦＡＸ１ = '" + sData[12] + "' \n"
							+       ",申込者ＦＡＸ２ = '" + sData[13] + "' \n"
							+       ",申込者ＦＡＸ３ = '" + sData[14] + "' \n"
							+       ",設置場所区分 = '" + sData[15] + "' \n"
							+       ",設置場所カナ = '" + sData[16] + "' \n"
							+       ",設置場所名 = '" + sData[17] + "' \n"
							+       ",設置場所郵便番号 = '" + sData[18] + "' \n"
							+       ",設置場所県ＣＤ = '" + sData[19] + "' \n"
							+       ",設置場所住所１ = '" + sData[20] + "' \n"
							+       ",設置場所住所２ = '" + sData[21] + "' \n"
							+       ",設置場所電話１ = '" + sData[22] + "' \n"
							+       ",設置場所電話２ = '" + sData[23] + "' \n"
							+       ",設置場所電話３ = '" + sData[24] + "' \n"
							+       ",設置場所ＦＡＸ１ = '" + sData[25] + "' \n"
							+       ",設置場所ＦＡＸ２ = '" + sData[26] + "' \n"
							+       ",設置場所ＦＡＸ３ = '" + sData[27] + "' \n"
							+       ",設置場所担当者名 = '" + sData[28] + "' \n"
							+       ",設置場所役職名 = '" + sData[29] + "' \n"
							+       ",設置場所使用料 =  " + sData[30] + "  \n"
							+       ",会員ＣＤ = '" + sData[31] + "' \n"
							+       ",使用開始日 = '" + sData[32] + "' \n"
							+       ",部門ＣＤ = '" + sData[33] + "' \n"
							+       ",部門名 = '" + sData[34] + "' \n"
							+       ",サーマル台数 =  " + sData[35] + "  \n"
							+       ",利用者ＣＤ = '" + sData[36] + "' \n"
							+       ",利用者名 = '" + sData[37] + "' \n"
							+       ",パスワード = '" + sData[38] + "' \n"
							+       ",承認状態ＦＧ = '" + sData[39] + "' \n"
							+       ",メモ = '" + sData[40] + "' \n"
							+       ",削除ＦＧ = '0' \n"
							+       ",登録日時 = " + s更新日時 + " \n"
// MOD 2007.01.27 東都）高木 会員申込への追加 START
//							+       ",登録ＰＧ = '申込登録' \n"
							+       ",登録ＰＧ = '" + s更新ＰＧ + "' \n"
// MOD 2007.01.27 東都）高木 会員申込への追加 END
							+       ",登録者 = '" + sData[42] + "' \n"
							+       ",更新日時 = " + s更新日時 + " \n"
// MOD 2007.01.27 東都）高木 会員申込への追加 START
//							+       ",更新ＰＧ = '申込登録' \n"
							+       ",更新ＰＧ = '" + s更新ＰＧ + "' \n"
// MOD 2007.01.27 東都）高木 会員申込への追加 END
							+       ",更新者 = '" + sData[42] + "' \n"
							+ " WHERE 管理番号 = '" + sData[0] + "' \n";

						CmdUpdate(sUser, conn2, cmdQuery);

						string sRet会員   = "";
						string sRet部門   = "";
						string sRet利用者 = "";
						//承認状態ＦＧが[3：承認済]の場合
						if(sData[39].Equals("3")){
							sRet会員 = Ins_Member2(sUser, conn2, sData, s更新日時);
							if(sRet会員.Length == 4){
								//部門マスタ追加
								sRet部門 = Ins_Section2(sUser, conn2, sData, s更新日時);
								if(sRet部門.Length == 4){
									//利用者マスタ追加
									sRet利用者 = Ins_User2(sUser, conn2, sData, s更新日時);
								}
							}
						}
						if(sRet会員.Length > 4){
							tran.Rollback();
							sRet[0] = "お客様：" + sRet会員;
						}else if(sRet部門.Length > 4){
							tran.Rollback();
							sRet[0] = "セクション：" + sRet部門;
						}else if(sRet利用者.Length > 4){
							tran.Rollback();
							sRet[0] = "ユーザー：" + sRet利用者;
						}else{
							tran.Commit();
							sRet[0] = "正常終了";

//							sRet[1] = s更新日時;
//							sRet[2] = sData[0];		//管理番号

						}
					}
					else
					{
						tran.Rollback();
						sRet[0] = "既に登録されています";
					}
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 申込情報更新
		 * 引数：管理番号、会員名...
		 * 戻値：ステータス、更新日時
		 *
		 *********************************************************************/
// ADD 2007.01.30 東都）高木 印刷済（申請中）以降の場合には、新しい受付ＮＯを採番 START
		private static string UPD_MOSIKOMI_SELECT
			= "SELECT 管理番号 "
			+ ", 店所ＣＤ "
			+ ", 申込者カナ "
			+ ", 申込者名 "
			+ ", 申込者郵便番号 \n"
			+ ", 申込者県ＣＤ "
			+ ", 申込者住所１ "
			+ ", 申込者住所２ "
			+ ", 申込者電話１ "
			+ ", 申込者電話２ \n"
			+ ", 申込者電話３ "
			+ ", 申込者電話 "
			+ ", 申込者ＦＡＸ１ "
			+ ", 申込者ＦＡＸ２ "
			+ ", 申込者ＦＡＸ３ \n"
			+ ", 設置場所区分 "
			+ ", 設置場所カナ "
			+ ", 設置場所名 "
			+ ", 設置場所郵便番号 "
			+ ", 設置場所県ＣＤ \n"
			+ ", 設置場所住所１ "
			+ ", 設置場所住所２ "
			+ ", 設置場所電話１ "
			+ ", 設置場所電話２ "
			+ ", 設置場所電話３ \n"
			+ ", 設置場所ＦＡＸ１ "
			+ ", 設置場所ＦＡＸ２ "
			+ ", 設置場所ＦＡＸ３ "
			+ ", 設置場所担当者名 "
			+ ", 設置場所役職名 \n"
			+ ", 設置場所使用料 "
			+ ", 会員ＣＤ "
			+ ", 使用開始日 "
			+ ", 部門ＣＤ "
			+ ", 部門名 \n"
			+ ", \"サーマル台数\" "
			+ ", 利用者ＣＤ "
			+ ", 利用者名 "
			+ ", \"パスワード\" "
			+ ", 承認状態ＦＧ \n"
			+ ", メモ "
			+ ", TO_CHAR(更新日時) "
			+ ", 更新者 \n"
			+ "FROM ＳＭ０５会員申込 \n"
			+ "";

		private static string UPD_MOSIKOMI_DELETE
			= "UPDATE ＳＭ０５会員申込 \n"
			+ "SET 削除ＦＧ = '1' \n"
			+ "";
// ADD 2007.01.30 東都）高木 印刷済（申請中）以降の場合には、新しい受付ＮＯを採番 END

		[WebMethod]
		public string[] Upd_Mosikomi(string[] sUser, string[] sData)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "申込情報更新開始");

			OracleConnection conn2 = null;
// DEL 2007.01.30 東都）高木 印刷済（申請中）以降の場合には、新しい受付ＮＯを採番 START
//			string[] sRet = new string[2]{"",""};
// DEL 2007.01.30 東都）高木 印刷済（申請中）以降の場合には、新しい受付ＮＯを採番 END
			string s更新日時 = System.DateTime.Now.ToString("yyyyMMddHHmmss");
// ADD 2007.01.30 東都）高木 印刷済（申請中）以降の場合には、新しい受付ＮＯを採番 START
			string[] sRet = new string[3]{"", s更新日時, sData[0]};
// ADD 2007.01.30 東都）高木 印刷済（申請中）以降の場合には、新しい受付ＮＯを採番 END

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";

			try
			{
// ADD 2007.01.30 東都）高木 印刷済（申請中）以降の場合には、新しい受付ＮＯを採番 START
				bool bUpdState = false;

				//承認状態ＦＧが[1：申請中]の場合（印刷ボタンの時）
				if(sData[39].Equals("1")){
					string[] sRefData = new string[43];
					cmdQuery = UPD_MOSIKOMI_SELECT
							+ " WHERE 管理番号 = '" + sData[0] + "' \n"
							+ " AND 削除ＦＧ = '0' \n"
							+ " AND 更新日時 = " + sData[41] + " \n"
							+ " FOR UPDATE \n";

					OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
					if(!reader.Read())
					{
						tran.Rollback();
						sRet[0] = "他の端末で更新されています";
						logWriter(sUser, INF, sRet[0]);
						return sRet;
					}
					sRefData[0] = "";
					//管理番号はダミー
					sRefData[1] = reader.GetString(1).Trim();
					sRefData[2] = reader.GetString(2).Trim();
					sRefData[3] = reader.GetString(3).Trim();
					sRefData[4] = reader.GetString(4).Trim();
					sRefData[5] = reader.GetString(5).Trim();	//申込者県ＣＤ
					sRefData[6] = reader.GetString(6).Trim();
					sRefData[7] = reader.GetString(7).Trim();
					sRefData[8] = reader.GetString(8).Trim();
					sRefData[9] = reader.GetString(9).Trim();
					sRefData[10] = reader.GetString(10).Trim();	//申込者電話３
					sRefData[11] = reader.GetString(11).Trim();
					sRefData[12] = reader.GetString(12).Trim();
					sRefData[13] = reader.GetString(13).Trim();
					sRefData[14] = reader.GetString(14).Trim();
					sRefData[15] = reader.GetString(15).Trim();	//設置場所区分
					sRefData[16] = reader.GetString(16).Trim();
					sRefData[17] = reader.GetString(17).Trim();
					sRefData[18] = reader.GetString(18).Trim();
					sRefData[19] = reader.GetString(19).Trim();
					sRefData[20] = reader.GetString(20).Trim();	//設置場所住所１
					sRefData[21] = reader.GetString(21).Trim();
					sRefData[22] = reader.GetString(22).Trim();
					sRefData[23] = reader.GetString(23).Trim();
					sRefData[24] = reader.GetString(24).Trim();
					sRefData[25] = reader.GetString(25).Trim();	//設置場所ＦＡＸ１
					sRefData[26] = reader.GetString(26).Trim();
					sRefData[27] = reader.GetString(27).Trim();
					sRefData[28] = reader.GetString(28).Trim();
					sRefData[29] = reader.GetString(29).Trim();
					sRefData[30] = reader.GetDecimal(30).ToString().Trim();	//設置場所使用料
					sRefData[31] = reader.GetString(31).Trim();
					sRefData[32] = reader.GetString(32).Trim();
					sRefData[33] = reader.GetString(33).Trim();
					sRefData[34] = reader.GetString(34).Trim();
					sRefData[35] = reader.GetDecimal(35).ToString().Trim();	//サーマル台数
					sRefData[36] = reader.GetString(36).Trim();
					sRefData[37] = reader.GetString(37).Trim();
					sRefData[38] = reader.GetString(38).Trim();
					sRefData[39] = reader.GetString(39).Trim();
					sRefData[40] = reader.GetString(40).Trim();	//メモ
					sRefData[41] = reader.GetString(41).Trim();
					sRefData[42] = reader.GetString(42).Trim();

					//承認状態ＦＧ（_:登録中、1:申請中、2:留保中、3:承認済）が
					//（1:申請中もしくは2:留保中のもの）
					if(sRefData[39].Length > 0){
						//データの更新状況をチェックする
						for(int iCnt = 2; iCnt <= 30; iCnt++){
							if(!sRefData[iCnt].Equals(sData[iCnt].Trim())){
								bUpdState = true;
								break;
							}
						}

						if(bUpdState){
							//データ削除
							cmdQuery = UPD_MOSIKOMI_DELETE
							+ ", 更新ＰＧ = '申込更新' \n"
							+ ", 更新者   = '" + sData[42] +"' \n"
							+ ", 更新日時 = "+ s更新日時 + " \n"
							+ " WHERE 管理番号 = '" + sData[0] + "' \n"
							+ " AND 削除ＦＧ = '0' \n"
							+ " AND 更新日時 = " + sData[41] + " \n";

							if (CmdUpdate(sUser, conn2, cmdQuery) == 0)
							{
								tran.Rollback();
								sRet[0] = "他の端末で更新されています";
							}else{
								tran.Commit();
								sRet[0] = "正常終了";
							}
							logWriter(sUser, INF, sRet[0]);
							//データが変更されている場合には、新しい受注ＮＯでデータを追加する
//保留　トランザクション制御
							return Ins_Mosikomi(sUser, sData);
						}
					}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
					disposeReader(reader);
					reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				}
// ADD 2007.01.30 東都）高木 印刷済（申請中）以降の場合には、新しい受付ＮＯを採番 END

				cmdQuery
					= "UPDATE ＳＭ０５会員申込 \n"
					+   " SET 店所ＣＤ = '" + sData[1] + "' \n"
					+       ",申込者カナ = '" + sData[2] + "' \n"
					+       ",申込者名 = '" + sData[3] + "' \n"
					+       ",申込者郵便番号 = '" + sData[4] + "' \n"
					+       ",申込者県ＣＤ = '" + sData[5] + "' \n"
					+       ",申込者住所１ = '" + sData[6] + "' \n"
					+       ",申込者住所２ = '" + sData[7] + "' \n"
					+       ",申込者電話１ = '" + sData[8] + "' \n"
					+       ",申込者電話２ = '" + sData[9] + "' \n"
					+       ",申込者電話３ = '" + sData[10] + "' \n"
					+       ",申込者電話 = '" + sData[11] + "' \n"
					+       ",申込者ＦＡＸ１ = '" + sData[12] + "' \n"
					+       ",申込者ＦＡＸ２ = '" + sData[13] + "' \n"
					+       ",申込者ＦＡＸ３ = '" + sData[14] + "' \n"
					+       ",設置場所区分 = '" + sData[15] + "' \n"
					+       ",設置場所カナ = '" + sData[16] + "' \n"
					+       ",設置場所名 = '" + sData[17] + "' \n"
					+       ",設置場所郵便番号 = '" + sData[18] + "' \n"
					+       ",設置場所県ＣＤ = '" + sData[19] + "' \n"
					+       ",設置場所住所１ = '" + sData[20] + "' \n"
					+       ",設置場所住所２ = '" + sData[21] + "' \n"
					+       ",設置場所電話１ = '" + sData[22] + "' \n"
					+       ",設置場所電話２ = '" + sData[23] + "' \n"
					+       ",設置場所電話３ = '" + sData[24] + "' \n"
					+       ",設置場所ＦＡＸ１ = '" + sData[25] + "' \n"
					+       ",設置場所ＦＡＸ２ = '" + sData[26] + "' \n"
					+       ",設置場所ＦＡＸ３ = '" + sData[27] + "' \n"
					+       ",設置場所担当者名 = '" + sData[28] + "' \n"
					+       ",設置場所役職名 = '" + sData[29] + "' \n"
					+       ",設置場所使用料 =  " + sData[30] + "  \n"
					+       ",会員ＣＤ = '" + sData[31] + "' \n"
					+       ",使用開始日 = '" + sData[32] + "' \n"
					+       ",部門ＣＤ = '" + sData[33] + "' \n"
					+       ",部門名 = '" + sData[34] + "' \n"
					+       ",サーマル台数 =  " + sData[35] + "  \n"
					+       ",利用者ＣＤ = '" + sData[36] + "' \n"
					+       ",利用者名 = '" + sData[37] + "' \n"
					+       ",パスワード = '" + sData[38] + "' \n"
					+       ",承認状態ＦＧ = '" + sData[39] + "' \n"
					+       ",メモ = '" + sData[40] + "' \n"
					+       ",更新日時 = " + s更新日時 + " \n"
					+       ",更新ＰＧ = '申込更新' \n"
					+       ",更新者 = '" + sData[42] + "' \n"
					+ " WHERE 管理番号 = '" + sData[0] + "' \n"
					+   " AND 削除ＦＧ = '0' \n"
					+   " AND 更新日時 = " + sData[41] + " \n";

				if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
					string sRet会員   = "";
					string sRet部門   = "";
					string sRet利用者 = "";
					//承認状態ＦＧが[3：承認済]の場合
					if(sData[39].Equals("3")){
						sRet会員 = Ins_Member2(sUser, conn2, sData, s更新日時);
						if(sRet会員.Length == 4){
							//部門マスタ追加
							sRet部門 = Ins_Section2(sUser, conn2, sData, s更新日時);
							if(sRet部門.Length == 4){
								//利用者マスタ追加
								sRet利用者 = Ins_User2(sUser, conn2, sData, s更新日時);
							}
						}
					}
					if(sRet会員.Length > 4){
						tran.Rollback();
						sRet[0] = "お客様：" + sRet会員;
					}else if(sRet部門.Length > 4){
						tran.Rollback();
						sRet[0] = "セクション：" + sRet部門;
					}else if(sRet利用者.Length > 4){
						tran.Rollback();
						sRet[0] = "ユーザー：" + sRet利用者;
					}else{
						tran.Commit();
						sRet[0] = "正常終了";
						sRet[1] = s更新日時;
					}
				}
				else
				{
					tran.Rollback();
					sRet[0] = "他の端末で更新されています";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
				logWriter(sUser, ERR, "StackTrace:\n" + ex.StackTrace);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 申込情報削除
		 * 引数：管理番号
		 * 戻値：ステータス
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Del_Mosikomi(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "申込情報削除開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "UPDATE ＳＭ０５会員申込 \n"
					+    "SET 削除ＦＧ = '1' "
					+       ",更新日時 = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
					+       ",更新ＰＧ = '申込削除' "
					+       ",更新者 = '" + sKey[2] + "' \n"
					+ " WHERE 管理番号 = '" + sKey[0] + "' "
					+   " AND 更新日時 = " + sKey[1] + " \n";

				if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
					tran.Commit();
					sRet[0] = "正常終了";
				}
				else
				{
					tran.Rollback();
					sRet[0] = "他の端末で更新されています";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 管理番号の採番
		 * 引数：会員ＣＤ、部門ＣＤ...
		 * 戻値：ステータス
		 *
		 *********************************************************************/
		[WebMethod]
		public String[] Get_KaniSaiban(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "管理番号の取得開始");
			
			OracleConnection conn2 = null;
			string[] sRet = new string[2]{"",""};
			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			//トランザクションの設定
			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			try
			{
				decimal iカレント番号 = 0;
				decimal i開始番号     = 0;
				decimal i終了番号     = 0;

				string cmdQuery
					= "SELECT カレント番号, 開始番号, 終了番号 \n"
					+ " FROM ＣＭ１６店所採番管理 \n"
					+ " WHERE 採番区分 = '01' \n"
					+ " AND 店所ＣＤ = '" + sKey[0] + "' \n"
					+ " FOR UPDATE \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);

				string updQuery = "";
				if(reader.Read())
				{
					iカレント番号 = reader.GetDecimal(0);
					i開始番号     = reader.GetDecimal(1);
					i終了番号     = reader.GetDecimal(2);

					if(iカレント番号 < i終了番号)
					{
						iカレント番号++;
					}else{
						iカレント番号 = i開始番号;
					}
					sRet[1] = iカレント番号.ToString("0000000");

					updQuery 
						= "UPDATE ＣＭ１６店所採番管理 SET \n"
						+ "  カレント番号 = " + iカレント番号 + " \n"
						+ ", 開始番号 = " + i開始番号 + " \n"
						+ ", 終了番号 = " + i終了番号 + " \n"
						+ ", 更新日時 = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') \n"
						+ ", 更新ＰＧ = '会員申込' \n"
						+ ", 更新者 = '" + sKey[1] + "' \n"
						+ " WHERE 採番区分 = '01' \n"
						+ " AND 店所ＣＤ = '" + sKey[0] + "' \n"
						+ " AND 削除ＦＧ = '0' \n";
				}else{
// MOD 2006.11.30 東都）高木 画面の調整 START
//					iカレント番号 = 1;
//					i開始番号     = 1;
					iカレント番号 = 5005001;
					i開始番号     = 1000001;
// MOD 2006.11.30 東都）高木 画面の調整 END
					i終了番号     = 9999999;
					sRet[1] = iカレント番号.ToString("0000000");

					// 送り状採番の追加
					updQuery 
						= "INSERT INTO ＣＭ１６店所採番管理 VALUES( \n"
						+ " '01' \n"
						+ ",'" + sKey[0] + "' \n"
						+ ", " + iカレント番号 + " \n"
						+ ", " + i開始番号 + " \n"
						+ ", " + i終了番号 + " \n"
						+ ",'0' \n"
						+ ", TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
						+ ",'会員申込' "
						+ ",'" + sKey[1] + "' \n"
						+ ", TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS') "
						+ ",'会員申込' "
						+ ",'" + sKey[1] + "' \n"
						+ ") \n";
				}
				CmdUpdate(sUser, conn2, updQuery);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				tran.Commit();
				sRet[0] = "正常終了";
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			
			return sRet;
		}

		/*********************************************************************
		 * 会員マスタ追加２
		 * 引数：会員ＣＤ、会員名...
		 * 戻値：ステータス
		 *
		 *********************************************************************/
		private string Ins_Member2(string[] sUser, OracleConnection conn2, 
												string[] sData, string sUpdateTime)
		{
			//会員マスタ追加
			string[] sKey = new string[4]{
				sData[31],	//会員ＣＤ
				sData[3],	//申込者名
				sData[32],	//使用開始日
				sData[42]	//登録者、更新者
			};

			string sRet = "";

			string cmdQuery = "";
			cmdQuery
				= "SELECT 削除ＦＧ \n"
				+   "FROM ＣＭ０１会員 \n"
				+  "WHERE 会員ＣＤ = '" + sKey[0] + "' \n"
				+    "FOR UPDATE \n";

			OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
			int iCnt = 1;
			string s削除ＦＧ = "";
			while (reader.Read())
			{
				s削除ＦＧ = reader.GetString(0);
				iCnt++;
			}
			if(iCnt == 1)
			{
				//追加
				cmdQuery
					= "INSERT INTO ＣＭ０１会員 \n"
					+ " VALUES ('" + sKey[0] + "' "		//会員ＣＤ
					+         ",'" + sKey[1] + "' "		//会員名
					+         ",'" + sKey[2] + "' "		//使用開始日
					+         ",'99999999' "			//使用終了日
					+         ",'0' \n"					//管理者区分
					+         ",'0' "
					+         ",'0' "
					+         ",'0' "
					+         ",'0' "
					+         ",'0' \n"
					+         ",'0' "
					+         ",'0' "
					+         ",' ' "
					+         ", 0 "
					+         ", 0 \n"
					+         ", 0 "
					+         ", 0 "
					+         ", 0 \n"
					+         ",'0' \n"
					+         "," + sUpdateTime
					+         ",'会員登録' "
					+         ",'" + sKey[3] + "' \n"
					+         "," + sUpdateTime
					+         ",'会員登録' "
					+         ",'" + sKey[3] + "' \n"
					+ " ) \n";

				CmdUpdate(sUser, conn2, cmdQuery);

				sRet = "正常終了";
			}
			else
			{
				//追加更新
				if (s削除ＦＧ.Equals("1"))
				{
					cmdQuery
						= "UPDATE ＣＭ０１会員 \n"
						+   " SET 会員名 = '" + sKey[1] + "' \n"
						+       ",使用開始日 = '" + sKey[2] + "' \n"
						+       ",使用終了日 = '99999999' \n"
						+       ",管理者区分 = '0' \n"
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 START
						+       ",記事連携ＦＧ = '0' \n"
						+       ",保留印刷ＦＧ = '0' \n"
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 END
						+       ",削除ＦＧ = '0' \n"
						+       ",登録日時 = " + sUpdateTime
						+       ",登録ＰＧ = '会員登録' "
						+       ",登録者 = '" + sKey[3] + "' \n"
						+       ",更新日時 = " + sUpdateTime
						+       ",更新ＰＧ = '会員登録' "
						+       ",更新者 = '" + sKey[3] + "' \n"
						+ " WHERE 会員ＣＤ = '" + sKey[0] + "' \n";

					CmdUpdate(sUser, conn2, cmdQuery);

					sRet = "正常終了";
				}
				else
				{
					sRet = "既に登録されています";
				}
			}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
			disposeReader(reader);
			reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
//			logWriter(sUser, INF, sRet);

			return sRet;
		}

		/*********************************************************************
		 * 部門マスタ追加２
		 * 引数：会員ＣＤ、部門ＣＤ...
		 * 戻値：ステータス
		 *
		 *********************************************************************/
		private string Ins_Section2(string[] sUser, OracleConnection conn2, 
											string[] sData, string sUpdateTime)
		{
// MOD 2009.03.03 東都）高木 サーマルシリアル番号の追加 START
//			string[] sKey = new string[8]{
			string[] sKey = new string[10]{
// MOD 2009.03.03 東都）高木 サーマルシリアル番号の追加 END
				sData[31],	//会員ＣＤ
				sData[33],	//部門ＣＤ
				sData[34],	//部門名
				sData[18],	//設置場所郵便番号
				sData[20],	//設置場所住所１
				sData[21],	//設置場所住所２
				sData[35],	//サーマル台数
				sData[42]	//登録者、更新者
// ADD 2009.03.03 東都）高木 サーマルシリアル番号の追加 START
				,sData[30]	//設置場所使用料
				,sData[0]	//管理番号
// ADD 2009.03.03 東都）高木 サーマルシリアル番号の追加 END
			};
			string sRet = "";

			string cmdQuery = "";

			cmdQuery
				= "SELECT 削除ＦＧ \n"
				+   "FROM ＣＭ０２部門 \n"
				+  "WHERE 会員ＣＤ = '" + sKey[0] + "' \n"
				+    "AND 部門ＣＤ = '" + sKey[1] + "' \n"
				+    "FOR UPDATE \n";

			OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
			int iCnt = 1;
			string s削除ＦＧ = "";
			while (reader.Read())
			{
				s削除ＦＧ = reader.GetString(0);
				iCnt++;
			}
			if(iCnt == 1)
			{
				//追加
				cmdQuery
					= "INSERT INTO ＣＭ０２部門 \n"
// ADD 2009.03.03 東都）高木 サーマルシリアル番号の追加 START
						+         "(会員ＣＤ \n"
						+         ",部門ＣＤ \n"
						+         ",部門名 \n"
						+         ",組織ＣＤ \n"
						+         ",出力順 \n"
						+         ",郵便番号 \n"
						+         ",\"ジャーナルＮＯ登録日\" \n"
						+         ",\"ジャーナルＮＯ管理\" \n"
						+         ",雛型ＮＯ \n"
						+         ",出荷日 \n"
						+         ",設置先住所１ \n"
						+         ",設置先住所２ \n"
						+         ",サーマル台数 \n"
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） START
//// ADD 2009.03.03 東都）高木 サーマルシリアル番号の追加 START
//						+         ",使用料 \n"
//						+         ",会員申込管理番号 \n"
//// ADD 2009.03.03 東都）高木 サーマルシリアル番号の追加 END
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） END
						+         ",削除ＦＧ \n"
						+         ",登録日時 \n"
						+         ",登録ＰＧ \n"
						+         ",登録者 \n"
						+         ",更新日時 \n"
						+         ",更新ＰＧ \n"
						+         ",更新者 \n"
						+         ") \n"
// ADD 2009.03.03 東都）高木 サーマルシリアル番号の追加 END
					+ " VALUES ('" + sKey[0] + "' "				//会員ＣＤ
					+         ",'" + sKey[1] + "' "				//部門ＣＤ
					+         ",'" + sKey[2] + "' "				//部門名
					+         ",' ' "							//組織ＣＤ
					+         ", 0 \n"							//出力順
					+         ",'" + sKey[3] + "' "				//郵便番号
					+         ",TO_CHAR(SYSDATE,'YYYYMMDD') "	//ジャーナルＮＯ登録日
					+         ", 0 "							//ジャーナル管理ＮＯ
					+         ", 0 "							//雛型ＮＯ
					+         ",TO_CHAR(SYSDATE,'YYYYMMDD') \n"	//出荷日
					+         ",'" + sKey[4] + "' "				//設置先住所１
					+         ",'" + sKey[5] + "' "				//設置先住所２
					+         ", " + sKey[6] + " \n"			//サーマル台数
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） START
//// ADD 2009.03.03 東都）高木 サーマルシリアル番号の追加 START
//					+         ", " + sKey[8] + " \n"			//使用料
//					+         ", " + sKey[9] + " \n"			//会員申込管理番号
//// ADD 2009.03.03 東都）高木 サーマルシリアル番号の追加 END
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） END
					+         ",'0' \n"
					+         "," + sUpdateTime
					+         ",'会員登録' "
					+         ",'" + sKey[7] + "' \n"
					+         "," + sUpdateTime
					+         ",'会員登録' "
					+         ",'" + sKey[7] + "' \n"
					+ " ) \n";

				CmdUpdate(sUser, conn2, cmdQuery);

// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） START
				cmdQuery
					= "INSERT INTO ＣＭ０６部門拡張 \n"
					+         "(会員ＣＤ \n"
					+         ",部門ＣＤ \n"
					+         ",使用料 \n"
					+         ",会員申込管理番号 \n"
					+         ",削除ＦＧ \n"
					+         ",登録日時 \n"
					+         ",登録ＰＧ \n"
					+         ",登録者 \n"
					+         ",更新日時 \n"
					+         ",更新ＰＧ \n"
					+         ",更新者 \n"
					+         ") \n"
					+ " VALUES ('" + sKey[0] + "' "				//会員ＣＤ
					+         ",'" + sKey[1] + "' "				//部門ＣＤ
					+         ", " + sKey[8] + " \n"			//使用料
					+         ", " + sKey[9] + " \n"			//会員申込管理番号
					+         ",'0' \n"
					+         "," + sUpdateTime
					+         ",'会員登録' "
					+         ",'" + sKey[7] + "' \n"
					+         "," + sUpdateTime
					+         ",'会員登録' "
					+         ",'" + sKey[7] + "' \n"
					+ " ) \n";
				CmdUpdate(sUser, conn2, cmdQuery);
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） END

				sRet = "正常終了";
			}
			else
			{
				//追加更新
				if (s削除ＦＧ.Equals("1"))
				{
					cmdQuery
						= "UPDATE ＣＭ０２部門 \n"
						+   " SET 部門名 = '" + sKey[2] + "' \n"
						+       ",組織ＣＤ = ' ' \n"
						+       ",出力順 = 0 \n"
						+       ",郵便番号 = '" + sKey[3] + "' \n"
						+       ",ジャーナルＮＯ登録日 = TO_CHAR(SYSDATE,'YYYYMMDD') \n"
						+       ",ジャーナルＮＯ管理 = 0 \n"
						+       ",雛型ＮＯ = 0 \n"
						+       ",出荷日 = TO_CHAR(SYSDATE,'YYYYMMDD') \n"
						+       ",設置先住所１ = '" + sKey[4] + "' \n"
						+       ",設置先住所２ = '" + sKey[5] + "' \n"
						+       ",サーマル台数 =  " + sKey[6] + " \n"
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） START
//// ADD 2009.03.03 東都）高木 サーマルシリアル番号の追加 START
//						+       ",使用料 = " + sKey[8] + " \n"
//						+       ",会員申込管理番号 = " + sKey[9] + " \n"
//// ADD 2009.03.03 東都）高木 サーマルシリアル番号の追加 END
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） END
						+       ",削除ＦＧ = '0' \n"
						+       ",登録日時 = " + sUpdateTime
						+       ",登録ＰＧ = '会員登録' "
						+       ",登録者 = '" + sKey[7] + "' \n"
						+       ",更新日時 = " + sUpdateTime
						+       ",更新ＰＧ = '会員登録' "
						+       ",更新者 = '" + sKey[7] + "'\n"
						+ " WHERE 会員ＣＤ = '" + sKey[0] + "' \n"
						+   " AND 部門ＣＤ = '" + sKey[1] + "' \n";

					CmdUpdate(sUser, conn2, cmdQuery);
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） START
					cmdQuery
						= "UPDATE ＣＭ０６部門拡張 SET \n"
						+       " 使用料 = " + sKey[8] + " \n"
						+       ",会員申込管理番号 = " + sKey[9] + " \n"
						+       ",削除ＦＧ = '0' \n"
						+       ",登録日時 = " + sUpdateTime
						+       ",登録ＰＧ = '会員登録' "
						+       ",登録者 = '" + sKey[7] + "' \n"
						+       ",更新日時 = " + sUpdateTime
						+       ",更新ＰＧ = '会員登録' "
						+       ",更新者 = '" + sKey[7] + "'\n"
						+ " WHERE 会員ＣＤ = '" + sKey[0] + "' \n"
						+   " AND 部門ＣＤ = '" + sKey[1] + "' \n";

					CmdUpdate(sUser, conn2, cmdQuery);
// MOD 2009.03.24 東都）高木 サーマルシリアル番号の追加（ＣＭ０６部門拡張） END
					sRet = "正常終了";
				}
				else
				{
					sRet = "既に登録されています";
				}
			}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
			disposeReader(reader);
			reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
//			logWriter(sUser, INF, sRet);

			//エラー時は、終了
			if (!sRet.Equals("正常終了")) return sRet;

			logWriter(sUser, INF, "記事の初期データ登録開始");

			//記事の初期データの検索
			cmdQuery
				= "SELECT 記事ＣＤ \n"
				+      ", 記事 \n"
				+   "FROM ＳＭ０３記事 \n"
				+  "WHERE 会員ＣＤ = 'default' \n"
				+    "AND 部門ＣＤ = '0000' \n"
				+    "AND 削除ＦＧ = '0' \n";

			OracleDataReader readerDef = CmdSelect(sUser, conn2, cmdQuery);
			string s初期記事ＣＤ = "";
			string s初期記事     = "";
			while (readerDef.Read())
			{
				s初期記事ＣＤ = readerDef.GetString(0);
				s初期記事     = readerDef.GetString(1);

				//記事の検索
				cmdQuery
					= "SELECT 記事ＣＤ \n"
					+   "FROM ＳＭ０３記事 \n"
					+  "WHERE 会員ＣＤ = '" + sKey[0] + "' \n"
					+    "AND 部門ＣＤ = '" + sKey[1] + "' \n"
					+    "AND 記事ＣＤ = '" + s初期記事ＣＤ + "' \n"
				    +    "FOR UPDATE \n";

				OracleDataReader readerNote = CmdSelect(sUser, conn2, cmdQuery);
				if (readerNote.Read())
				{
					//既に記事がある場合は新規更新
					cmdQuery
						= "UPDATE ＳＭ０３記事 \n"
						+   " SET 記事 = '" + s初期記事 + "' \n"
						+       ",削除ＦＧ = '0' \n"
						+       ",登録日時 = " + sUpdateTime
						+       ",登録ＰＧ = '初期記事' \n"
						+       ",登録者 = '" + sKey[7] + "' \n"
						+       ",更新日時 = " + sUpdateTime
						+       ",更新ＰＧ = '初期記事' \n"
						+       ",更新者 = '" + sKey[7] + "' \n"
						+ " WHERE 会員ＣＤ = '" + sKey[0] + "' \n"
						+   " AND 部門ＣＤ = '" + sKey[1] + "' \n"
						+   " AND 記事ＣＤ = '" + s初期記事ＣＤ + "' \n";

					CmdUpdate(sUser, conn2, cmdQuery);
					sRet = "正常終了";
				}
				else
				{
					//新規追加
					cmdQuery
						= "INSERT INTO ＳＭ０３記事 \n"
						+ " VALUES ('" + sKey[0] + "' " 
						+         ",'" + sKey[1] + "' "
						+         ",'" + s初期記事ＣＤ + "' "
						+         ",'" + s初期記事 + "' \n"
						+         ",'0' \n"
						+         "," + sUpdateTime
						+         ",'初期記事' "
						+         ",'" + sKey[7] + "' \n"
						+         "," + sUpdateTime
						+         ",'初期記事' "
						+         ",'" + sKey[7] + "' \n"
						+ " ) \n";

					CmdUpdate(sUser, conn2, cmdQuery);
					sRet = "正常終了";
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(readerNote);
				readerNote = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
//				logWriter(sUser, INF, sRet);
			}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
			disposeReader(readerDef);
			readerDef = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END

			return sRet;
		}

		/*********************************************************************
		 * 利用者マスタ追加２
		 * 引数：会員ＣＤ、利用者ＣＤ、利用者名
		 * 戻値：ステータス
		 *
		 *********************************************************************/
		private string Ins_User2(string[] sUser, OracleConnection conn2, 
											string[] sData, string sUpdateTime)
		{
			string[] sKey = new string[6]{
				sData[31],	//会員ＣＤ
				sData[36],	//利用者ＣＤ
				sData[38],	//パスワード
				sData[37],	//利用者名
				sData[33],	//部門ＣＤ
				sData[42]	//登録者、更新者
			};
			string sRet = "";

			string cmdQuery = "";

			cmdQuery
				= "SELECT 削除ＦＧ \n"
				+   "FROM ＣＭ０４利用者 \n"
				+  "WHERE 会員ＣＤ = '" + sKey[0] + "' \n"
				+    "AND 利用者ＣＤ = '" + sKey[1] + "' \n"
				+    "FOR UPDATE \n";

			OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
			int iCnt = 1;
			string s削除ＦＧ = "";
			while (reader.Read())
			{
				s削除ＦＧ = reader.GetString(0);
				iCnt++;
			}
			if(iCnt == 1)
			{
				//追加
				cmdQuery
					= "INSERT INTO ＣＭ０４利用者 \n"
					+ " VALUES ('" + sKey[0] + "' "		//会員ＣＤ
					+         ",'" + sKey[1] + "' "		//利用者ＣＤ
					+         ",'" + sKey[2] + "' "		//パスワード
					+         ",'" + sKey[3] + "' "		//利用者名
					+         ",'" + sKey[4] + "' \n"	//部門ＣＤ
					+         ",' ' "					//荷送人ＣＤ
					+         ",0 "						//認証エラー回数
					+         ",' ' "					//権限１
					+         ",' ' "
					+         ",' ' \n"
					+         ",' ' "
					+         ",' ' "
					+         ",' ' "
					+         ",' ' "
					+         ",' ' \n"
					+         ",' ' "
					+         ",' ' \n"
					+         ",'0' \n"
					+         "," + sUpdateTime
// MOD 2008.05.21 東都）高木 ログインエラー回数を５回にする START
//					+         ",'会員登録' "
					+         ",'"+ sUpdateTime.Substring(0,8) +"' "
// MOD 2008.05.21 東都）高木 ログインエラー回数を５回にする END
					+         ",'" + sKey[5] + "' \n"
					+         "," + sUpdateTime
					+         ",'会員登録' "
					+         ",'" + sKey[5] + "' \n"
					+ " ) \n";

				CmdUpdate(sUser, conn2, cmdQuery);
				sRet = "正常終了";
			}
			else
			{
				//追加更新
				if (s削除ＦＧ.Equals("1"))
				{
					cmdQuery
						= "UPDATE ＣＭ０４利用者 \n"
						+   " SET パスワード = '" + sKey[2] + "' \n"
						+       ",利用者名 = '" + sKey[3] + "' \n"
						+       ",部門ＣＤ = '" + sKey[4] + "' \n"
						+       ",荷送人ＣＤ = ' ' \n"
						+       ",認証エラー回数 = 0 \n"
						+       ",権限１ = ' ' \n"
						+       ",削除ＦＧ = '0' \n"
						+       ",登録日時 = " + sUpdateTime
// MOD 2008.05.21 東都）高木 ログインエラー回数を５回にする START
//						+       ",登録ＰＧ = '会員登録' "
						+       ",登録ＰＧ = '"+ sUpdateTime.Substring(0,8) +"' "
// MOD 2008.05.21 東都）高木 ログインエラー回数を５回にする END
						+       ",登録者 = '" + sKey[5] + "' \n"
						+       ",更新日時 = " + sUpdateTime
						+       ",更新ＰＧ = '会員登録' "
						+       ",更新者 = '" + sKey[5] + "' \n"
						+ " WHERE 会員ＣＤ = '" + sKey[0] + "' \n"
						+   " AND 利用者ＣＤ = '" + sKey[1] + "' \n";

					CmdUpdate(sUser, conn2, cmdQuery);
					sRet = "正常終了";
				}
				else
				{
					sRet = "既に登録されています";
				}
			}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
			disposeReader(reader);
			reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
//			logWriter(sUser, INF, sRet);

			return sRet;
		}
// ADD 2006.08.03 東都）高木 会員申込機能の追加 END  
// ADD 2006.08.22 東都）山本 正式店所名の取得の追加 START  
		/*********************************************************************
		 * 郵便番号マスタ取得
		 * 引数：郵便番号
		 * 戻値：ステータス、住所、店所正式名、店所ＣＤ
		 *
		 * 参照元：会員加入.cs		[]	
		 * 参照元：店所情報.cs		[]	
		 * 参照元：請求先マスタ.cs	[]	
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_Postcode1(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "郵便番号マスタ検索開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[5];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT NVL(CM10.店所名, ' '), \n"
					+ " TRIM(CM14.都道府県名) || TRIM(CM14.市区町村名) || TRIM(CM14.町域名),TRIM(CM10.店所正式名),TRIM(CM10.店所ＣＤ) \n"
//保留：王子			+ " TRIM(CM14.都道府県名) || TRIM(CM14.市区町村名) || TRIM(CM14.町域名),NVL(TRIM(CM10.店所正式名), ' '),TRIM(CM14.店所ＣＤ) \n"
					+  " FROM ＣＭ１４郵便番号 CM14 \n"
					+  " LEFT JOIN ＣＭ１０店所 CM10 \n"
					+    " ON CM14.店所ＣＤ = CM10.店所ＣＤ "
					+    "AND CM10.削除ＦＧ = '0' \n"
					+ " WHERE CM14.郵便番号 = '" + sKey[0] + "' \n"
//注意 MOD 2010.04.13 東都）高木 郵便番号が削除された時の障害対応 START
//					+   " AND CM14.削除ＦＧ = '0' \n";
//注意 MOD 2010.04.13 東都）高木 郵便番号が削除された時の障害対応 END
					;

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				while (reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();
					sRet[2] = reader.GetString(1).Trim();
					sRet[3] = reader.GetString(2).Trim();
					sRet[4] = reader.GetString(3).Trim();
					iCnt++;
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				if(iCnt == 1) 
					sRet[0] = "該当データがありません";
				else
					sRet[0] = "正常終了";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}
// ADD 2006.08.22 東都）山本 正式店所名の取得の追加 END  


// ADD 2006.09.06 東都）山本 契約受付店所情報の取得の追加 START  
		/*********************************************************************
		 * 店所マスタ取得
		 * 引数：
		 * 戻値：ステータス、店所名
		 *
		 *********************************************************************/
// ADD 2006.11.06 東都）高木 店所情報画面の追加 START
		private static string GET_SHOP_INF_SELECT
			= "SELECT 契約書店所名称 \n"
			+       ",契約書住所都道府県 \n"
			+       ",契約書住所１ \n"
			+       ",契約書住所２ \n"
			+       ",契約書郵便番号 \n"
			+       ",契約書電話番号 \n"
			+       ",契約書ＦＡＸ番号 \n"
			+       ",地区１ \n"
			+       ",地区２ \n"
			+       ",TO_CHAR(更新日時) \n"
			+       " FROM ＣＭ１０店所 \n";
// ADD 2006.11.06 東都）高木 店所情報画面の追加 END
		[WebMethod]
		public string[] Get_ShopInf(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "店所マスタ検索開始");

			OracleConnection conn2 = null;
// MOD 2006.11.06 東都）高木 店所情報画面の追加 START
//			string[] sRet = new string[8];
			string[] sRet = new string[11];
// MOD 2006.11.06 東都）高木 店所情報画面の追加 END

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			string cmdQuery = "";
			try
			{
// MOD 2006.11.06 東都）高木 店所情報画面の追加 START
//				cmdQuery
//					= "SELECT CM10.契約書店所名称, \n"
//                                      +  "      CM10.契約書住所都道府県 , \n"
//                                      +  "      CM10.契約書住所１ , \n"
//                                      +  "      CM10.契約書住所２ , \n"
//                                      +  "      CM10.契約書郵便番号 , \n"
//                                      +  "      CM10.契約書電話番号 , \n"
//                                      +  "      CM10.契約書ＦＡＸ番号 \n"
//					+  " FROM ＣＭ１０店所 CM10 \n"
//					+ " WHERE CM10.店所ＣＤ = '" + sKey[0] + "' \n"
//					+    "AND CM10.削除ＦＧ = '0' \n";
				cmdQuery
					= GET_SHOP_INF_SELECT
					+ " WHERE 店所ＣＤ = '" + sKey[0] + "' \n"
					+    "AND 削除ＦＧ = '0' \n";
// MOD 2006.11.06 東都）高木 店所情報画面の追加 END

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				while (reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();
					sRet[2] = reader.GetString(1).Trim();
					sRet[3] = reader.GetString(2).Trim();
					sRet[4] = reader.GetString(3).Trim();
					sRet[5] = reader.GetString(4).Trim();
					sRet[6] = reader.GetString(5).Trim();
					sRet[7] = reader.GetString(6).Trim();
// ADD 2006.11.06 東都）高木 店所情報画面の追加 START
					sRet[8] = reader.GetString(7).Trim();
					sRet[9] = reader.GetString(8).Trim();
					sRet[10] = reader.GetString(9).Trim();
// ADD 2006.11.06 東都）高木 店所情報画面の追加 END
					iCnt++;
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				if(iCnt == 1) 
					sRet[0] = "該当データがありません";
				else
					sRet[0] = "正常終了";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}
// ADD 2006.09.06 東都）山本 契約受付店所情報の取得の追加 END  

		// MOD 2006.11.06 東都）高木 店所情報画面の追加 START
		/*********************************************************************
		 * 店所情報更新
		 * 引数：管理番号、会員名...
		 * 戻値：ステータス、更新日時
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Upd_ShopInf(string[] sUser, string[] sData)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "店所情報更新開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[2]{"",""};
			string s更新日時 = System.DateTime.Now.ToString("yyyyMMddHHmmss");

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
// MOD 2014.09.10 BEVAS)前田 支店止め機能追加 START
				string s福通支店止めＦＧ;
				string s王子支店止めＦＧ;
				if (sData.Length > 12) 
				{
					s福通支店止めＦＧ = sData[12];
					s王子支店止めＦＧ = sData[13];
				} 
				else 
				{
					s福通支店止めＦＧ = "0";
					s王子支店止めＦＧ = "0";
				}
				cmdQuery
					= "UPDATE ＣＭ１０店所 \n"
					+   " SET 契約書店所名称 = '" + sData[1] + "' \n"
					+       ",契約書住所都道府県 = '" + sData[2] + "' \n"
					+       ",契約書住所１ = '" + sData[3] + "' \n"
					+       ",契約書住所２ = '" + sData[4] + "' \n"
					+       ",契約書郵便番号 = '" + sData[5] + "' \n"
					+       ",契約書電話番号 = '" + sData[6] + "' \n"
					+       ",契約書ＦＡＸ番号 = '" + sData[7] + "' \n"
					+       ",地区１ = '" + sData[8] + "' \n"
					+       ",地区２ = '" + sData[9] + "' \n"
					+       ",更新日時 = " + s更新日時 + " \n"
					+       ",更新ＰＧ = '店所更新' \n"
					+       ",更新者 = '" + sData[11] + "' \n"
					+       ",支店止めＦＧ１ = '" + s福通支店止めＦＧ + "' \n"
					+       ",支店止めＦＧ２ = '" + s王子支店止めＦＧ + "' \n"
					+ " WHERE 店所ＣＤ = '" + sData[0] + "' \n"
					+   " AND 削除ＦＧ = '0' \n"
					+   " AND 更新日時 = " + sData[10] + " \n";
// MOD 2014.09.10 BEVAS)前田 支店止め機能追加 END

				if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
					tran.Commit();
					sRet[0] = "正常終了";
					sRet[1] = s更新日時;
				}
				else
				{
					tran.Rollback();
					sRet[0] = "他の端末で更新されています";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}
// MOD 2006.11.06 東都）高木 店所情報画面の追加 END
// ADD 2006.11.08 東都）山本 会員マスタを店所コードで絞り込む START
		/*********************************************************************
		 * 会員マスタ取得
		 * 引数：会員ＣＤ
		 * 戻値：ステータス、会員ＣＤ、会員名、使用開始日、管理者区分、使用終了日
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_MemberTn(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "会員マスタ検索開始");

			OracleConnection conn2 = null;
// MOD 2009.05.28 東都）高木 出荷実績一覧運賃非表示対応 START
//			string[] sRet = new string[7];
			string[] sRet = new string[8];
// MOD 2009.05.28 東都）高木 出荷実績一覧運賃非表示対応 END
			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT CM01.会員ＣＤ "
					+       ",CM01.会員名 "
					+       ",CM01.使用開始日 "
					+       ",CM01.管理者区分 "
					+       ",CM01.使用終了日 "
					+       ",CM01.更新日時 \n"
// MOD 2009.05.28 東都）高木 出荷実績一覧運賃非表示対応 START
					+       ",CM01.記事連携ＦＧ \n"
// MOD 2009.05.28 東都）高木 出荷実績一覧運賃非表示対応 END
					+  " FROM ＣＭ０１会員 CM01\n"
					+  "     ,ＣＭ０２部門 CM02\n"
					+  "     ,ＣＭ１４郵便番号 CM14\n"
					+ " WHERE CM01.会員ＣＤ = '" + sKey[0] + "' \n"
					+    "AND CM01.削除ＦＧ = '0' \n"
					+    "AND CM01.会員ＣＤ = CM02.会員ＣＤ \n"
					+    "AND CM02.削除ＦＧ = '0' \n"
					+    "AND CM14.郵便番号 = CM02.郵便番号 \n"
					+    "AND CM14.店所ＣＤ = '" + sKey[1] + "' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				while (reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();
					sRet[2] = reader.GetString(1).Trim();
					sRet[3] = reader.GetString(2).Trim();
					sRet[4] = reader.GetString(3).Trim();
					sRet[5] = reader.GetString(4).Trim();
					sRet[6] = reader.GetDecimal(5).ToString().Trim();
// MOD 2009.05.28 東都）高木 出荷実績一覧運賃非表示対応 START
					sRet[7] = reader.GetString(6);
// MOD 2009.05.28 東都）高木 出荷実績一覧運賃非表示対応 END
					iCnt++;
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				if(iCnt == 1) 
					sRet[0] = "該当データがありません";
				else
					sRet[0] = "正常終了";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 会員マスタ一覧取得２
		 * 引数：会員ＣＤ、会員名
		 * 戻値：ステータス、会員ＣＤ、会員名
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Get_MemberTn(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "会員マスタ一覧取得２開始");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT 会員.会員情報 from ( "
					+ "SELECT '|' "
					+     "|| TRIM(CM01.会員ＣＤ) || '|' "
// MOD 2010.04.27 東都）高木 運用中のお客様のみ対象機能の追加 START
//					+     "|| TRIM(CM01.会員名) || '|' 会員情報 \n"
					+     "|| TRIM(CM01.会員名) || '|' "
					+     "|| TRIM(使用終了日) || '|' "
					+     "|| TO_CHAR(SYSDATE,'YYYYMMDD') || '|' "
					+     "会員情報 \n"
// MOD 2010.04.27 東都）高木 運用中のお客様のみ対象機能の追加 END
					+  " FROM ＣＭ０１会員 CM01\n"
					+  "     ,ＣＭ０２部門 CM02 \n"
					+  "     ,ＣＭ１４郵便番号 CM14 \n";
				if (sKey[0].Trim().Length == 12)
				{
					cmdQuery += " WHERE CM01.会員ＣＤ = '" + sKey[0] + "' \n";
				}
				else
				{
					cmdQuery += " WHERE CM01.会員ＣＤ LIKE '" + sKey[0] + "%' \n";
				}
				if (sKey[1].Trim().Length != 0)
				{
					cmdQuery += " AND CM01.会員名 LIKE '%" + sKey[1] + "%' \n";
				}
// ADD 2010.12.14 ACT）垣原 王子運送の対応 START
				cmdQuery += " AND CM01.管理者区分 IN ('0','1','2') \n";
// ADD 2010.12.14 ACT）垣原 王子運送の対応 END
				cmdQuery += " AND CM01.削除ＦＧ = '0' \n";
				cmdQuery += " AND CM01.会員ＣＤ = CM02.会員ＣＤ \n";
				cmdQuery += " AND CM02.削除ＦＧ = '0' \n";
				cmdQuery += " AND CM14.郵便番号 = CM02.郵便番号 \n";
				if (sKey[2].Trim().Length != 0)
				{
					cmdQuery += " AND CM14.店所ＣＤ = '" + sKey[2] + "' \n";
				}
// MOD 2010.04.27 東都）高木 運用中のお客様のみ対象機能の追加 START
				if(sKey.Length >= 4){
					if(sKey[3] == "1"){
						cmdQuery += " AND CM01.使用終了日 >= TO_CHAR(SYSDATE,'YYYYMMDD') \n";
					}
				}
// MOD 2010.04.27 東都）高木 運用中のお客様のみ対象機能の追加 END
				cmdQuery += " ORDER BY CM01.会員ＣＤ \n";
				cmdQuery += " ) 会員 GROUP BY 会員.会員情報 \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);

				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0)
				{
					sRet[0] = "該当データがありません";
				}
				else
				{
					sRet[0] = "正常終了";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}
// ADD 2006.11.08 東都）山本 会員マスタを店所コードで絞り込む END

// ADD 2007.11.12 東都）高木 会員マスタ一覧の機能追加 START
		/*********************************************************************
		 * 会員マスタ一覧取得３
		 * 引数：会員ＣＤ、会員名
		 * 戻値：ステータス、会員ＣＤ、会員名
		 *
		 * 参照元：会員検索２.cs
		 *********************************************************************/
		[WebMethod]
		public string[] Get_MemberTn3(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "会員マスタ一覧取得３開始");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' "
					+     "|| TRIM(CM01.会員ＣＤ) || '|' "
// MOD 2010.04.27 東都）高木 運用中のお客様のみ対象機能の追加 START
//					+     "|| TRIM(CM01.会員名) || '|' 会員情報 \n"
					+     "|| TRIM(CM01.会員名) || '|' "
					+     "|| TRIM(使用終了日) || '|' "
					+     "|| TO_CHAR(SYSDATE,'YYYYMMDD') || '|' "
					+     "会員情報 \n"
// MOD 2010.04.27 東都）高木 運用中のお客様のみ対象機能の追加 END
					+     ", CM01.会員ＣＤ kcd \n"
					+  " FROM ＣＭ０１会員 CM01\n";
// MOD 2007.11.14 KCL) 森本 global対応 START
				cmdQuery += "     ,ＣＭ０２部門 CM02 \n";
				cmdQuery += "     ,ＣＭ１４郵便番号 CM14 \n";
// MOD 2007.11.14 KCL) 森本 global対応 END
				if (sKey[0].Trim().Length == 12)
				{
					cmdQuery += " WHERE CM01.会員ＣＤ = '" + sKey[0] + "' \n";
				}
				else
				{
					cmdQuery += " WHERE CM01.会員ＣＤ LIKE '" + sKey[0] + "%' \n";
				}
				if (sKey[1].Trim().Length != 0)
				{
					cmdQuery += " AND CM01.会員名 LIKE '%" + sKey[1] + "%' \n";
				}
// ADD 2010.12.14 ACT）垣原 王子運送の対応 START
				cmdQuery += " AND CM01.管理者区分 IN ('0','1','2') \n";
// ADD 2010.12.14 ACT）垣原 王子運送の対応 END
				cmdQuery += " AND CM01.削除ＦＧ = '0' \n";

// MOD 2007.11.14 KCL) 森本 global対応 START
				cmdQuery += " AND CM01.会員ＣＤ = CM02.会員ＣＤ \n"
					+ " AND CM02.削除ＦＧ = '0' \n"
					+ " AND CM02.郵便番号 = CM14.郵便番号 \n"
// MOD 2010.04.13 東都）高木 郵便番号が削除された時の障害対応 START
//					+ " AND CM14.削除ＦＧ = '0' \n";
// MOD 2010.04.13 東都）高木 郵便番号が削除された時の障害対応 END
					;
				if (sKey[2].Trim().Length != 0)
					cmdQuery += " AND CM14.店所ＣＤ = '" + sKey[2] + "' \n";
// MOD 2010.04.27 東都）高木 運用中のお客様のみ対象機能の追加 START
				if(sKey.Length >= 4){
					if(sKey[3] == "1"){
						cmdQuery += " AND CM01.使用終了日 >= TO_CHAR(SYSDATE,'YYYYMMDD') \n";
					}
				}
// MOD 2010.04.27 東都）高木 運用中のお客様のみ対象機能の追加 END
				cmdQuery += "UNION \n";
				cmdQuery += "SELECT '|' "
					+ "|| TRIM(CM01.会員ＣＤ) || '|' "
					+ "|| TRIM(CM01.会員名) || '|' 会員情報 \n"
					+ ", CM01.会員ＣＤ kcd \n"
					+ " FROM ＣＭ０１会員 CM01 \n"
					+ "     ,ＣＭ０５会員扱店 CM05 \n";
				if (sKey[0].Trim().Length == 12)
				{
					cmdQuery += " WHERE CM01.会員ＣＤ = '" + sKey[0] + "' \n";
				}
				else
				{
					cmdQuery += " WHERE CM01.会員ＣＤ LIKE '" + sKey[0] + "%' \n";
				}
				if (sKey[1].Trim().Length != 0)
				{
					cmdQuery += " AND CM01.会員名 LIKE '%" + sKey[1] + "%' \n";
				}
//保留：王子対応は？
				cmdQuery += " AND CM01.削除ＦＧ = '0' \n"
					+ " AND CM01.会員ＣＤ = CM05.会員ＣＤ \n"
					+ " AND CM05.削除ＦＧ = '0' \n";
				if (sKey[2].Trim().Length != 0)
					cmdQuery += " AND CM05.店所ＣＤ = '" + sKey[2] + "' \n";
// MOD 2010.04.27 東都）高木 運用中のお客様のみ対象機能の追加 START
				if(sKey.Length >= 4){
					if(sKey[3] == "1"){
						cmdQuery += " AND CM01.使用終了日 >= TO_CHAR(SYSDATE,'YYYYMMDD') \n";
					}
				}
// MOD 2010.04.27 東都）高木 運用中のお客様のみ対象機能の追加 END

// MOD 2016.04.08 bevas) 松本 社内伝票機能追加対応 START
				cmdQuery += "UNION \n";
				cmdQuery += "SELECT '|' "
					+ "|| TRIM(CM01.会員ＣＤ) || '|' "
					+ "|| TRIM(CM01.会員名) || '|' 会員情報 \n"
					+ ", CM01.会員ＣＤ kcd \n"
					+ " FROM ＣＭ０１会員 CM01 \n"
					+ "     ,ＣＭ０５会員扱店Ｆ CM05F \n";
				if (sKey[0].Trim().Length == 12)
				{
					cmdQuery += " WHERE CM01.会員ＣＤ = '" + sKey[0] + "' \n";
				}
				else
				{
					cmdQuery += " WHERE CM01.会員ＣＤ LIKE '" + sKey[0] + "%' \n";
				}
				if (sKey[1].Trim().Length != 0)
				{
					cmdQuery += " AND CM01.会員名 LIKE '%" + sKey[1] + "%' \n";
				}
//保留：王子対応は？
				cmdQuery += " AND CM01.削除ＦＧ = '0' \n"
					+ " AND CM01.会員ＣＤ = CM05F.会員ＣＤ \n"
					+ " AND CM05F.削除ＦＧ = '0' \n";
				if (sKey[2].Trim().Length != 0)
				{
					cmdQuery += " AND CM05F.店所ＣＤ = '" + sKey[2] + "' \n";
				}
				if(sKey.Length >= 4)
				{
					if(sKey[3] == "1")
					{
						cmdQuery += " AND CM01.使用終了日 >= TO_CHAR(SYSDATE,'YYYYMMDD') \n";
					}
				}
// MOD 2016.04.08 bevas) 松本 社内伝票機能追加対応 END

				cmdQuery += " ORDER BY kcd \n";
// MOD 2007.11.14 KCL) 森本 global対応 END

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);

				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}

				disposeReader(reader);
				reader = null;

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0)
				{
					sRet[0] = "該当データがありません";
				}
				else
				{
					sRet[0] = "正常終了";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}
// ADD 2007.11.12 東都）高木 会員マスタ一覧の機能追加 END

// ADD 2006.12.08 東都）小童谷 発店所名取得 START
		/*********************************************************************
		 * 発店所名取得
		 * 引数：発店所ＣＤ
		 * 戻値：ステータス、店所正式名
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_Hatuten(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "発店所名検索開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[2];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT 店所正式名 "
					+   "FROM ＣＭ１０店所 "
					+  "WHERE 店所ＣＤ = '" + sKey[0] + "' "
					+    "AND 削除ＦＧ = '0' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				while (reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();
					iCnt++;
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				if(iCnt == 1) 
					sRet[0] = "該当データがありません";
				else
					sRet[0] = "正常終了";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}
// ADD 2006.12.08 東都）小童谷 発店所名取得 END

// ADD 2006.12.07 東都）小童谷 発店仕分けコード一覧表の機能追加 START
		/*********************************************************************
		 * 発店仕分けコード一覧表
		 * 引数：会員情報、発店所コード
		 * 戻値：ステータス、店所コード、店所名、仕分けコード、...
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Get_Tyakusiwake(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "発店仕分けコード一覧取得開始");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' "
					+     "|| CM10.店所ＣＤ || '|' "
					+     "|| CM10.店所正式名 || '|' "
					+     "|| TRIM(NVL(CM17.仕分ＣＤ,' ')) || '|' "
					+     "|| TRIM(NVL(CM17.仕分ＣＤ,' ')) || '|' "
					+     "|| NVL(CM17.更新日時,'0') || '|' \n"
					+  " FROM ＣＭ１０店所 CM10,ＣＭ１７仕分 CM17  \n"
					+ " WHERE CM10.店所ＣＤ = CM17.着店所ＣＤ(+) \n"
					+   " AND '" + sKey[0] + "' = CM17.発店所ＣＤ(+) \n"
// MOD 2015.10.09 bevas）松本 発店仕分けコード登録画面に自店所と東京テリトリ店を表示 START
//					+   " AND CM10.店所ＣＤ >= '100' \n"
//					+   " AND CM10.店所ＣＤ <> '" + sKey[0] + "' \n"
					+   " AND CM10.店所ＣＤ >= '080' \n"
// MOD 2015.10.09 bevas）松本 発店仕分けコード登録画面に自店所と東京テリトリ店を表示 END
					+   " AND CM10.削除ＦＧ = '0' \n"
					+   " AND '0' = CM17.削除ＦＧ(+) \n"
					+   " ORDER BY CM10.店所ＣＤ \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "該当データがありません";
				else
				{
					sRet[0] = "正常終了";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}
// ADD 2006.12.07 東都）小童谷 発店仕分けコード一覧表の機能追加 END
// ADD 2006.12.08 東都）小童谷 発店仕分け登録 START
		/*********************************************************************
		 * 発店仕分け追加
		 * 引数：発店所ＣＤ、着店所ＣＤ、仕分けＣＤ...
		 * 戻値：ステータス
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Ins_TyakuSiwake(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "発店仕分け追加開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];
			string s更新日時 = System.DateTime.Now.ToString("yyyyMMddHHmmss");

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				OracleDataReader reader;
// MOD 2007.03.15 FJCS）桑田 バグ修正 START
//				for(short sCnt = 0; sCnt < 17; sCnt++)
				for(short sCnt = 0; sCnt < 16; sCnt++)
// MOD 2007.03.15 FJCS）桑田 バグ修正 END
				{
					if(sKey[sCnt] == null)
					{
						sCnt = 20;
					}
					else
					{
						string[] sData = sKey[sCnt].Split(',');

						cmdQuery
							= "SELECT 削除ＦＧ "
							+   "FROM ＣＭ１７仕分 "
							+  "WHERE 発店所ＣＤ = '" + sData[0] + "' "
							+  "  AND 着店所ＣＤ = '" + sData[1] + "' "
							+    "FOR UPDATE \n";

						reader = CmdSelect(sUser, conn2, cmdQuery);
						int iCnt = 1;
						string s削除ＦＧ = "";
						while (reader.Read())
						{
							s削除ＦＧ = reader.GetString(0);
							iCnt++;
						}
						if(iCnt == 1)
						{
							//追加
							cmdQuery
								= "INSERT INTO ＣＭ１７仕分 \n"
								+ " VALUES ('" + sData[0] + "' " 
								+         ",'" + sData[1] + "' "
								+         ",'" + sData[2] + "' "
								+         ",'0' "
								+         "," + s更新日時
								+         ",'" + sData[3] + "' "
								+         ",'" + sData[4] + "' "
								+         "," + s更新日時
								+         ",'" + sData[3] + "' "
								+         ",'" + sData[4] + "' "
								+ " ) \n";

							CmdUpdate(sUser, conn2, cmdQuery);

							sRet[0] = "正常終了";
						}
						else
						{
							//追加更新
							if (s削除ＦＧ.Equals("1"))
							{
								cmdQuery
									= "UPDATE ＣＭ１７仕分 \n"
									+   " SET 仕分ＣＤ = '" + sData[2] + "' "
									+       ",削除ＦＧ = '0' \n"
									+       ",登録日時 = " + s更新日時
									+       ",登録ＰＧ = '" + sData[3] + "' "
									+       ",登録者   = '" + sData[4] + "' "
									+       ",更新日時 = " + s更新日時
									+       ",更新ＰＧ = '" + sData[3] + "' "
									+       ",更新者   = '" + sData[4] + "' \n"
									+  "WHERE 発店所ＣＤ = '" + sData[0] + "' "
									+  "  AND 着店所ＣＤ = '" + sData[1] + "' \n";

								CmdUpdate(sUser, conn2, cmdQuery);

								sRet[0] = "正常終了";
							}
							else
							{
								cmdQuery
									= "UPDATE ＣＭ１７仕分 \n"
									+   " SET 仕分ＣＤ = '" + sData[2] + "' "
									+       ",削除ＦＧ = '0' \n"
									+       ",更新日時 = " + s更新日時
									+       ",更新ＰＧ = '" + sData[3] + "' "
									+       ",更新者   = '" + sData[4] + "' \n"
									+  "WHERE 発店所ＣＤ = '" + sData[0] + "' "
									+  "  AND 着店所ＣＤ = '" + sData[1] + "' \n";

								CmdUpdate(sUser, conn2, cmdQuery);

								sRet[0] = "正常終了";
							}

						}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
						disposeReader(reader);
						reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
					}
				}
				tran.Commit();
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}
// ADD 2006.12.08 東都）小童谷 発店仕分け登録 END
// ADD 2006.12.12 東都）高木 状態情報取得 START
		/*********************************************************************
		 * 状態一覧取得
		 * 引数：なし
		 * 戻値：ステータス、状態一覧
		 *********************************************************************/
		private static string GET_JYOTAI_COUNT
			= "SELECT COUNT(ROWID) \n"
			+  " FROM ＡＭ０３状態 \n"
			+ " WHERE 状態詳細ＣＤ = ' ' \n"
			+ " AND 削除ＦＧ = '0' \n";

		private static string GET_JYOTAI
			= "SELECT 状態ＣＤ, 状態名 \n"
			+  " FROM ＡＭ０３状態 \n"
			+ " WHERE 状態詳細ＣＤ = ' ' \n"
			+ " AND 削除ＦＧ = '0' \n"
			+ " ORDER BY 状態ＣＤ, 状態名 \n";

		[WebMethod]
		public string[] Get_jyotai(string[] sUser )
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "状態一覧取得開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}

			string s状態数 = "0";
			int    i状態数 = 0;
			try
			{
				OracleDataReader reader = CmdSelect(sUser, conn2, GET_JYOTAI_COUNT);
				if (reader.Read() )
				{
//					s状態数 = reader.GetString(0);
					s状態数 = reader.GetDecimal(0).ToString().Trim();
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				i状態数 = int.Parse(s状態数);

				reader = CmdSelect(sUser, conn2, GET_JYOTAI);

				int iPos = 2;
				sRet = new string[i状態数 * 2 + iPos];
				while (reader.Read() && iPos < sRet.Length)
				{
					sRet[iPos++] = reader.GetString(0).Trim();
					sRet[iPos++] = reader.GetString(1).Trim();
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END

				if(iPos > 2){
					sRet[0] = "正常終了";
					sRet[1] = s状態数;
				}else{
					sRet[0] = "サーバエラー：状態マスタが設定されていません";
					sRet[1] = "0";
				}

				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}

			return sRet;
		}
// ADD 2006.12.12 東都）高木 状態情報取得 END
// ADD 2006.12.15 東都）小童谷 ご依頼主一覧 START
		/*********************************************************************
		 * ご依頼主一覧取得
		 * 引数：会員ＣＤ、荷送人名、荷送人ＣＤ、店所ＣＤ
		 * 戻値：ステータス、一覧（名前１、住所１、荷送人ＣＤ）...
		 *
		 * 参照元：ご依頼主検索.cs
		 *********************************************************************/
		[WebMethod]
		public string[] Get_Goirainusi(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "ご依頼主一覧取得開始");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' "
					+     "|| TRIM(SM01.会員ＣＤ) || '|' "
					+     "|| TRIM(CM01.会員名) || '|' "
					+     "|| TRIM(CM02.部門名) || '|' "
					+     "|| TRIM(SM01.荷送人ＣＤ) || '|' "
					+     "|| TRIM(SM01.名前１) || '|' "
					+     "|| TRIM(SM01.住所１) || '|' "
					+     "|| TRIM(SM01.部門ＣＤ) || '|' \n"
					+  " FROM ＳＭ０１荷送人 SM01"
					+       ",ＣＭ０２部門 CM02"
					+       ",ＣＭ１４郵便番号 CM14"
					+       ",ＣＭ０１会員 CM01 \n"
					+ " WHERE SM01.会員ＣＤ =  CM01.会員ＣＤ \n";
				if (sKey[0].Length == 10)
				{
					cmdQuery += " AND SM01.会員ＣＤ = '" + sKey[0] + "' \n";
				}
				else
				{
					cmdQuery += " AND SM01.会員ＣＤ LIKE '" + sKey[0] + "%' \n";
				}
				if (sKey[1].Length == 12)
				{
					cmdQuery += " AND SM01.荷送人ＣＤ = '" + sKey[1] + "' \n";
				}
				else
				{
					if (sKey[1].Length != 0)
					{
						cmdQuery += " AND SM01.荷送人ＣＤ LIKE '" + sKey[1] + "%' \n";
					}
				}
				if (sKey[2].Length != 0)
				{
					cmdQuery += " AND SM01.名前１ LIKE '%" + sKey[2] + "%' \n";
				}
				cmdQuery += " AND SM01.会員ＣＤ =  CM02.会員ＣＤ \n"
					     +  " AND SM01.部門ＣＤ =  CM02.部門ＣＤ \n"
					     +  " AND CM02.郵便番号 =  CM14.郵便番号 \n"
					     +  " AND CM14.店所ＣＤ =  '" + sKey[3] + "' \n";
				cmdQuery += " AND SM01.削除ＦＧ = '0' \n"
						 +  " AND CM02.削除ＦＧ = '0' \n"
// MOD 2010.04.13 東都）高木 郵便番号が削除された時の障害対応 START
//						 +  " AND CM14.削除ＦＧ = '0' \n"
// MOD 2010.04.13 東都）高木 郵便番号が削除された時の障害対応 END
						 +  " AND CM01.削除ＦＧ = '0' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "該当データがありません";
				else
				{
					sRet[0] = "正常終了";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}
// ADD 2006.12.15 東都）小童谷 ご依頼主一覧 END
// ADD 2007.11.14 KCL) 森本 ご依頼主一覧２ START
		/*********************************************************************
		 * ご依頼主一覧取得（global対応）
		 * 引数：会員ＣＤ、荷送人名、荷送人ＣＤ、店所ＣＤ
		 * 戻値：ステータス、一覧（名前１、住所１、荷送人ＣＤ）...
		 *
		 * 参照元：ご依頼主検索２.cs
		 *********************************************************************/
		[WebMethod]
		public string[] Get_Goirainusi2(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "ご依頼主一覧取得２開始");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' "
					+     "|| TRIM(SM01.会員ＣＤ) || '|' "
					+     "|| TRIM(CM01.会員名) || '|' "
					+     "|| TRIM(CM02.部門名) || '|' "
					+     "|| TRIM(SM01.荷送人ＣＤ) || '|' "
					+     "|| TRIM(SM01.名前１) || '|' "
					+     "|| TRIM(SM01.住所１) || '|' "
					+     "|| TRIM(SM01.部門ＣＤ) || '|' \n"
					+    ",CM01.会員ＣＤ kcd \n"
					+  " FROM ＳＭ０１荷送人 SM01"
					+       ",ＣＭ０２部門 CM02"
					+       ",ＣＭ１４郵便番号 CM14"
					+       ",ＣＭ０１会員 CM01 \n"
					+ " WHERE SM01.会員ＣＤ =  CM01.会員ＣＤ \n";
				if (sKey[0].Length == 10)
				{
					cmdQuery += " AND SM01.会員ＣＤ = '" + sKey[0] + "' \n";
				}
				else
				{
					cmdQuery += " AND SM01.会員ＣＤ LIKE '" + sKey[0] + "%' \n";
				}
				if (sKey[1].Length == 12)
				{
					cmdQuery += " AND SM01.荷送人ＣＤ = '" + sKey[1] + "' \n";
				}
				else
				{
					if (sKey[1].Length != 0)
					{
						cmdQuery += " AND SM01.荷送人ＣＤ LIKE '" + sKey[1] + "%' \n";
					}
				}
				if (sKey[2].Length != 0)
				{
					cmdQuery += " AND SM01.名前１ LIKE '%" + sKey[2] + "%' \n";
				}
				cmdQuery += " AND SM01.会員ＣＤ =  CM02.会員ＣＤ \n"
					+  " AND SM01.部門ＣＤ =  CM02.部門ＣＤ \n"
					+  " AND CM02.郵便番号 =  CM14.郵便番号 \n"
// MOD 2009.05.11 東都）高木 未出荷対応 START
//					+  " AND CM14.店所ＣＤ =  '" + sKey[3] + "' \n";
					;
				if (sKey[3].Length != 0){
					cmdQuery += " AND CM14.店所ＣＤ =  '" + sKey[3] + "' \n";
				}
// MOD 2009.05.11 東都）高木 未出荷対応 END
				cmdQuery += " AND SM01.削除ＦＧ = '0' \n"
					+  " AND CM02.削除ＦＧ = '0' \n"
// MOD 2010.04.13 東都）高木 郵便番号が削除された時の障害対応 START
//					+  " AND CM14.削除ＦＧ = '0' \n"
// MOD 2010.04.13 東都）高木 郵便番号が削除された時の障害対応 END
					+  " AND CM01.削除ＦＧ = '0' \n";

				cmdQuery += "UNION \n";
				cmdQuery += "SELECT '|' "
					+     "|| TRIM(SM01.会員ＣＤ) || '|' "
					+     "|| TRIM(CM01.会員名) || '|' "
					+     "|| TRIM(CM02.部門名) || '|' "
					+     "|| TRIM(SM01.荷送人ＣＤ) || '|' "
					+     "|| TRIM(SM01.名前１) || '|' "
					+     "|| TRIM(SM01.住所１) || '|' "
					+     "|| TRIM(SM01.部門ＣＤ) || '|' \n"
					+    ",CM01.会員ＣＤ kcd \n"
					+  " FROM ＳＭ０１荷送人 SM01"
					+       ",ＣＭ０２部門 CM02"
					+       ",ＣＭ０５会員扱店 CM05"
					+       ",ＣＭ０１会員 CM01 \n"
					+ " WHERE SM01.会員ＣＤ =  CM01.会員ＣＤ \n"
					+ "";
				if (sKey[0].Length == 10)
				{
					cmdQuery += " AND SM01.会員ＣＤ = '" + sKey[0] + "' \n";
				}
				else
				{
					cmdQuery += " AND SM01.会員ＣＤ LIKE '" + sKey[0] + "%' \n";
				}
				if (sKey[1].Length == 12)
				{
					cmdQuery += " AND SM01.荷送人ＣＤ = '" + sKey[1] + "' \n";
				}
				else
				{
					if (sKey[1].Length != 0)
					{
						cmdQuery += " AND SM01.荷送人ＣＤ LIKE '" + sKey[1] + "%' \n";
					}
				}
				if (sKey[2].Length != 0)
				{
					cmdQuery += " AND SM01.名前１ LIKE '%" + sKey[2] + "%' \n";
				}
				cmdQuery += " AND SM01.会員ＣＤ =  CM02.会員ＣＤ \n"
					+  " AND SM01.部門ＣＤ =  CM02.部門ＣＤ \n"
					+  " AND SM01.会員ＣＤ =  CM05.会員ＣＤ \n"
// MOD 2009.05.11 東都）高木 未出荷対応 START
//					+  " AND CM05.店所ＣＤ =  '" + sKey[3] + "' \n";
					;
				if (sKey[3].Length != 0){
					cmdQuery += " AND CM05.店所ＣＤ =  '" + sKey[3] + "' \n";
				}
// MOD 2009.05.11 東都）高木 未出荷対応 END
				cmdQuery += " AND SM01.削除ＦＧ = '0' \n"
					+  " AND CM02.削除ＦＧ = '0' \n"
					+  " AND CM05.削除ＦＧ = '0' \n"
					+  " AND CM01.削除ＦＧ = '0' \n";
// MOD 2016.04.08 bevas) 松本 社内伝票機能追加対応 START
				cmdQuery += "UNION \n";
				cmdQuery += "SELECT '|' "
					+     "|| TRIM(SM01.会員ＣＤ) || '|' "
					+     "|| TRIM(CM01.会員名) || '|' "
					+     "|| TRIM(CM02.部門名) || '|' "
					+     "|| TRIM(SM01.荷送人ＣＤ) || '|' "
					+     "|| TRIM(SM01.名前１) || '|' "
					+     "|| TRIM(SM01.住所１) || '|' "
					+     "|| TRIM(SM01.部門ＣＤ) || '|' \n"
					+    ",CM01.会員ＣＤ kcd \n"
					+  " FROM ＳＭ０１荷送人 SM01"
					+       ",ＣＭ０２部門 CM02"
					+       ",ＣＭ０５会員扱店Ｆ CM05F"
					+       ",ＣＭ０１会員 CM01 \n"
					+ " WHERE SM01.会員ＣＤ =  CM01.会員ＣＤ \n"
					+ "";
				if(sKey[0].Length == 10)
				{
					cmdQuery += " AND SM01.会員ＣＤ = '" + sKey[0] + "' \n";
				}
				else
				{
					cmdQuery += " AND SM01.会員ＣＤ LIKE '" + sKey[0] + "%' \n";
				}
				if(sKey[1].Length == 12)
				{
					cmdQuery += " AND SM01.荷送人ＣＤ = '" + sKey[1] + "' \n";
				}
				else
				{
					if(sKey[1].Length != 0)
					{
						cmdQuery += " AND SM01.荷送人ＣＤ LIKE '" + sKey[1] + "%' \n";
					}
				}
				if(sKey[2].Length != 0)
				{
					cmdQuery += " AND SM01.名前１ LIKE '%" + sKey[2] + "%' \n";
				}
				cmdQuery += " AND SM01.会員ＣＤ =  CM02.会員ＣＤ \n"
					+  " AND SM01.部門ＣＤ =  CM02.部門ＣＤ \n"
					+  " AND SM01.会員ＣＤ =  CM05F.会員ＣＤ \n"
					;
				if(sKey[3].Length != 0)
				{
					cmdQuery += " AND CM05F.店所ＣＤ =  '" + sKey[3] + "' \n";
				}
				cmdQuery += " AND SM01.削除ＦＧ = '0' \n"
					+  " AND CM02.削除ＦＧ = '0' \n"
					+  " AND CM05F.削除ＦＧ = '0' \n"
					+  " AND CM01.削除ＦＧ = '0' \n";
// MOD 2016.04.08 bevas) 松本 社内伝票機能追加対応 END

				cmdQuery += "ORDER BY kcd \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while (reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
				disposeReader(reader);
				reader = null;

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "該当データがありません";
				else
				{
					sRet[0] = "正常終了";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}
// ADD 2007.11.14 KCL) 森本 ご依頼主一覧２ END
// ADD 2006.12.18 東都）小童谷 出荷一覧取得 START
		/*********************************************************************
		 * 出荷一覧取得
		 * 引数：会員ＣＤ、部門ＣＤ、荷送人ＣＤ、出荷日 or 登録日、
		 *		 開始日、終了日、状態、送状番号
		 * 戻値：ステータス、一覧（出荷日、住所１、名前１、）...
		 *
		 *********************************************************************/
		private static string GET_SYUKKA_UNION_1 
			= "SELECT  \n"
			+       " COUNT(*), \n"
			+       " NVL(SUM(A.個数),0), \n"
			+       " NVL(SUM(A.重量),0), \n"
			+       " NVL(SUM(A.才数),0)  \n"
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 START
			+       ", NVL(SUM(A.運賃重量),0) \n"
			+       ", NVL(SUM(A.運賃才数),0) \n"
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 END
// MOD 2007.01.11 東都）高木 送り状番号で検索した時、お客様名、依頼主名を表示 START
//			+       " FROM (  \n";
			+       "";
// MOD 2007.01.11 東都）高木 送り状番号で検索した時、お客様名、依頼主名を表示 END

		private static string GET_SYUKKA_INDEX_1 
// MOD 2009.05.11 東都）高木 未出荷対応 START
//			= "SELECT /*+ INDEX(S ST01IDX1) INDEX(J AM03PKEY) */ \n";
			= "SELECT \n";
// MOD 2009.05.11 東都）高木 未出荷対応 END

		private static string GET_SYUKKA_INDEX_2 
// MOD 2009.05.11 東都）高木 未出荷対応 START
//			= "SELECT /*+ INDEX(S ST01IDX2) INDEX(J AM03PKEY) */ \n";
			= "SELECT \n";
// MOD 2009.05.11 東都）高木 未出荷対応 END

		private static string GET_SYUKKA_INDEX_3 
// MOD 2009.05.11 東都）高木 未出荷対応 START
//			= "SELECT /*+ INDEX(S ST02IDX1) INDEX(J AM03PKEY) */ \n";
			= "SELECT \n";
// MOD 2009.05.11 東都）高木 未出荷対応 END

		private static string GET_SYUKKA_INDEX_4 
// MOD 2009.05.11 東都）高木 未出荷対応 START
//			= "SELECT /*+ INDEX(S ST02IDX2) INDEX(J AM03PKEY) */ \n";
			= "SELECT \n";
// MOD 2009.05.11 東都）高木 未出荷対応 END

		private static string GET_SYUKKA_SELECT_1 
			=       " NVL(S.個数,0) AS 個数, \n"
			+       " NVL(S.重量,0) AS 重量, \n"
			+       " NVL(S.才数,0) AS 才数 \n"
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 START
			+       ", NVL(DECODE(S.運賃重量,'     ',0,S.運賃重量),0) AS 運賃重量 \n"
			+       ", NVL(DECODE(S.運賃才数,'     ',0,S.運賃才数),0) AS 運賃才数 \n"
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 END
			;
//			+  " FROM \"ＳＴ０１出荷ジャーナル\" S, ＡＭ０３状態 J \n";

		private static string GET_SYUKKA_SELECT_2 
			=       " SUBSTR(S.出荷日,5,2) || '/' || SUBSTR(S.出荷日,7,2), S.住所１, S.名前１, \n"
			+       " TO_CHAR(S.個数), S.重量, S.輸送指示１, \n"
			+       " S.品名記事１, S.送り状番号, DECODE(S.元着区分,1,'元払',2,'着払',S.元着区分), \n"
			+       " DECODE(S.指定日,0,' ',(SUBSTR(S.指定日,5,2) || '/' || SUBSTR(S.指定日,7,2) || DECODE(S.指定日区分,'0','必着','1','指定',''))), \n"

			+       " DECODE(S.詳細状態,'  ', NVL(J.状態名, S.状態),NVL(J.状態詳細名, S.詳細状態)), \n"
			+       " SUBSTR(S.登録日,5,2) || '/' || SUBSTR(S.登録日,7,2), \n"
			+       " S.お客様出荷番号, TO_CHAR(S.\"ジャーナルＮＯ\") AS 管理番号, S.登録日, \n"
			+       " SUBSTR(S.出荷日,1,4) || '/' || SUBSTR(S.出荷日,5,2) || '/' || SUBSTR(S.出荷日,7,2), \n"
			+       " S.登録者, \n"
			+       " S.才数, \n"
// MOD 2007.02.20 東都）高木 保険料の表示 START
//			+       " S.保険金額, \n"
			+       " S.諸料金, \n"
// MOD 2007.02.20 東都）高木 保険料の表示 END
// MOD 2007.10.22 東都）高木 運賃に中継料を加算表示 START
//			+       " S.運賃, \n"
			+       " S.運賃 + S.中継, \n"
// MOD 2007.10.22 東都）高木 運賃に中継料を加算表示 END
// ADD 2007.01.17 東都）高木 一覧項目に削除ＦＧ、送り状発行済ＦＧを表示 START
			+       " DECODE(S.削除ＦＧ,'1','削',' '), \n"
			+       " DECODE(S.送り状発行済ＦＧ,'1','済',' '), \n"
// ADD 2007.01.17 東都）高木 一覧項目に削除ＦＧ、送り状発行済ＦＧを表示 END
// MOD 2007.11.22 東都）高木 一覧項目に発店ＣＤを表示 START
			+       " S.発店ＣＤ, \n"
// MOD 2007.11.22 東都）高木 一覧項目に発店ＣＤを表示 END
			+       " S.出荷日 \n"
// ADD 2008.12.01 東都）高木 出荷照会の一覧のソート順の訂正 START
			+       ", S.\"ジャーナルＮＯ\" \n"
// ADD 2008.12.01 東都）高木 出荷照会の一覧のソート順の訂正 END
// MOD 2009.05.11 東都）高木 未出荷対応 START
			+       ", S.会員ＣＤ \n"
			+       ", NVL(CM01.会員名, ' ') \n"
			+       ", S.部門ＣＤ \n"
// MOD 2009.05.11 東都）高木 未出荷対応 END
// MOD 2009.09.11 東都）高木 出荷照会で出荷済ＦＧ,送信済ＦＧなどを追加 START
			+       ", S.出荷済ＦＧ, S.送信済ＦＧ, S.登録日時 \n"
			+       ", S.更新日時, S.更新ＰＧ, S.更新者 \n"
// MOD 2009.09.11 東都）高木 出荷照会で出荷済ＦＧ,送信済ＦＧなどを追加 END
// MOD 2010.04.06 東都）高木 出荷照会に得意先、郵便番号などを追加 START
			+       ",S.得意先ＣＤ, S.部課ＣＤ, S.部課名 \n"
			+       ",S.郵便番号, S.着店ＣＤ, S.着店名 \n"
			+       ",S.荷送人ＣＤ, S.登録ＰＧ \n"
// MOD 2010.04.06 東都）高木 出荷照会に得意先、郵便番号などを追加 END
// MOD 2010.10.12 東都）高木 運賃エラー対応 START
			+       ", S.\"運賃エラー確認ＦＧ\" \n"
// MOD 2010.10.12 東都）高木 運賃エラー対応 END
// MOD 2010.11.25 東都）高木 出荷照会に削除日時などを追加 START
			+       ", S.削除日時, S.削除ＰＧ, S.削除者 \n"
// MOD 2010.11.25 東都）高木 出荷照会に削除日時などを追加 END
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 START
			+       ", S.運賃才数, S.運賃重量 \n"
			+       ", NVL(CM01.保留印刷ＦＧ,'0') \n"
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 END
// MOD 2014.03.19 BEVAS）高杉 配完日付・時刻を追加 START
			+       ", DECODE(S.処理０３,'          ',' ',('20' || SUBSTR(S.処理０３,1,2) || '/' || SUBSTR(S.処理０３,3,2) || '/' || SUBSTR(S.処理０３,5,2) || ' ' || SUBSTR(S.処理０３,7,2) || ':' || SUBSTR(S.処理０３,9,2))) \n"
// MOD 2014.03.19 BEVAS）高杉 配完日付・時刻を追加 END
			;
// MOD 2011.05.20 東都）高木 出荷照会件数の検索上限解除 START
		private static string GET_SYUKKA_SELECT_22 
			=       ", S.名前２ \n"
			+       ", S.品名記事２, S.品名記事３ \n"
// MOD 2011.07.28 東都）高木 記事行の追加 START
			+       ", S.品名記事４, S.品名記事５, S.品名記事６ \n"
// MOD 2011.07.28 東都）高木 記事行の追加 END
			;
// MOD 2011.05.20 東都）高木 出荷照会件数の検索上限解除 END

//			+ " FROM \"ＳＴ０１出荷ジャーナル\" S, ＡＭ０３状態 J \n";

		private static string GET_SYUKKA_FROM_1 
// MOD 2009.05.11 東都）高木 未出荷対応 START
//			= " FROM \"ＳＴ０１出荷ジャーナル\" S, ＡＭ０３状態 J \n";
			= " FROM \"ＳＴ０１出荷ジャーナル\" S \n"
			+ ", ＡＭ０３状態 J \n"
			+ ", ＣＭ０１会員 CM01 \n"
			;
// MOD 2009.05.11 東都）高木 未出荷対応 END

		private static string GET_SYUKKA_FROM_2 
// MOD 2009.05.11 東都）高木 未出荷対応 START
//			= " FROM \"ＳＴ０２出荷履歴\" S, ＡＭ０３状態 J \n";
			= " FROM \"ＳＴ０２出荷履歴\" S \n"
			+ ", ＡＭ０３状態 J \n"
			+ ", ＣＭ０１会員 CM01 \n"
			;
// MOD 2009.05.11 東都）高木 未出荷対応 END

// MOD 2008.12.01 東都）高木 出荷照会の一覧のソート順の訂正 START
//		private static string GET_SYUKKA_SELECT_2_SORT
//			= " ORDER BY A.登録日,A.管理番号 ";
//
//		private static string GET_SYUKKA_SELECT_2_SORT2
//			= " ORDER BY A.出荷日,A.登録日,A.管理番号 ";
// MOD 2009.05.11 東都）高木 未出荷対応 START
//		private static string GET_SYUKKA_SELECT_2_SORT
//			= " ORDER BY A.登録日, A.\"ジャーナルＮＯ\" ";
//
//		private static string GET_SYUKKA_SELECT_2_SORT2
//			= " ORDER BY A.出荷日, A.登録日, A.\"ジャーナルＮＯ\" ";

		private static string GET_SYUKKA_SELECT_2_SORT
			= " ORDER BY A.会員ＣＤ, A.部門ＣＤ, A.登録日, A.\"ジャーナルＮＯ\" ";

		private static string GET_SYUKKA_SELECT_2_SORT2
			= " ORDER BY A.出荷日, A.会員ＣＤ, A.部門ＣＤ, A.登録日, A.\"ジャーナルＮＯ\" ";
// MOD 2009.05.11 東都）高木 未出荷対応 END
// MOD 2008.12.01 東都）高木 出荷照会の一覧のソート順の訂正 END

		[WebMethod]
		public String[] Get_syukka(string[] sUser, string[] sKey)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "出荷一覧取得開始");

			OracleConnection conn2 = null;
// MOD 2007.01.11 東都）高木 送り状番号で検索した時、お客様名、依頼主名を表示 START
//			string[] sRet = new string[4];
			string[] sRet = new string[10]{"", "0", "0", "0", ""
											,"" ,"" ,"" ,"" ,""};
// MOD 2007.01.11 東都）高木 送り状番号で検索した時、お客様名、依頼主名を表示 END
			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			string s登録件数 = "0";
			string s個数合計 = "0";
			int    i登録件数 = 0;
			decimal d重量合計 = 0;
			decimal d才数合計 = 0;
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 START
			decimal d運賃重量計 = 0;
			decimal d運賃才数計 = 0;
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 END
			string s送り状    = "";
// ADD 2007.01.11 東都）高木 送り状番号で検索した時、お客様名、依頼主名を表示 START
			string s会員ＣＤ = "";
			string s部門ＣＤ = "";
			string s荷送人ＣＤ = "";
// ADD 2007.01.11 東都）高木 送り状番号で検索した時、お客様名、依頼主名を表示 END
// MOD 2009.05.11 東都）高木 未出荷対応 START
			bool   b未出荷 = sKey[6].Equals("90");
// MOD 2009.05.11 東都）高木 未出荷対応 END
//保留 MOD 2010.10.12 東都）高木 運賃エラー対応 START
//保留			bool   b運賃エラー = sKey[6].Equals("91");
//保留			if(b運賃エラー) sKey[8] = "1";
//保留 MOD 2010.10.12 東都）高木 運賃エラー対応 END
// MOD 2011.04.13 東都）高木 重量入力不可対応 START
			string  s運賃才数 = "";
			string  s運賃重量 = "";
			decimal d才数重量 = 0;
// MOD 2011.04.13 東都）高木 重量入力不可対応 END
// MOD 2011.05.20 東都）高木 出荷照会件数の検索上限解除 START
			string  s検索上限解除 = (sKey.Length > 10) ? sKey[10] : "0";
			bool    b検索上限解除 = (s検索上限解除 == "1") ? true : false;
// MOD 2011.05.20 東都）高木 出荷照会件数の検索上限解除 END

			StringBuilder sbQuery = new StringBuilder(1024);
			StringBuilder sbQuery2 = new StringBuilder(1024);
			StringBuilder sbRet = new StringBuilder(1024);
			try
			{
				if(sKey[7].Length == 0)
				{
// MOD 2009.05.11 東都）高木 未出荷対応 START
//					sbQuery.Append(" WHERE S.会員ＣＤ = '" + sKey[0] + "' \n");
					if(b未出荷){
						sbQuery.Append(", ＳＴ０５未出荷分 ST05 \n");
						if(sKey[0].Length > 0){
							sbQuery.Append(" WHERE ST05.会員ＣＤ = '" + sKey[0] + "' \n");
						}else{
							sbQuery.Append(" WHERE ST05.会員ＣＤ > ' ' \n");
						}
						if(sKey[1].Length > 0)
						{
							sbQuery.Append(" AND ST05.部門ＣＤ = '" + sKey[1] + "' \n");
						}
						if(sKey[3] == "0")
						{
							sbQuery.Append(" AND ST05.出荷日  BETWEEN '"+ sKey[4] + "' AND '"+ sKey[5] +"' \n");
						}
						else
						{
							sbQuery.Append(" AND ST05.登録日  BETWEEN '"+ sKey[4] + "' AND '"+ sKey[5] +"' \n");
						}
						sbQuery.Append(" AND ST05.出荷日 < TO_CHAR(SYSDATE,'YYYYMMDD') \n");
						sbQuery.Append(" AND ST05.状態   = '02' \n");
						if(sKey[9].Length > 0){
							sbQuery.Append(" AND ST05.発店ＣＤ = '"+ sKey[9] + "' \n");
						}
						sbQuery.Append(" AND ST05.会員ＣＤ = S.会員ＣＤ \n");
						sbQuery.Append(" AND ST05.部門ＣＤ = S.部門ＣＤ \n");
						sbQuery.Append(" AND ST05.登録日 = S.登録日 \n");
						sbQuery.Append(" AND ST05.\"ジャーナルＮＯ\" = S.\"ジャーナルＮＯ\" \n");
					}else{
						if(sKey[0].Length > 0){
							sbQuery.Append(" WHERE S.会員ＣＤ = '" + sKey[0] + "' \n");
						}else{
							sbQuery.Append(" WHERE S.会員ＣＤ > ' ' \n");
						}
						if(sKey[1].Length > 0)
						{
							sbQuery.Append("   AND S.部門ＣＤ = '" + sKey[1] + "' \n");
						}
					}
// MOD 2009.05.11 東都）高木 未出荷対応 END

					if(sKey[2].Length > 0)
					{
						sbQuery.Append(" AND S.荷送人ＣＤ = '"+ sKey[2] + "' \n");
					}

					if(sKey[3] == "0")
					{
						sbQuery.Append(" AND S.出荷日  BETWEEN '"+ sKey[4] + "' AND '"+ sKey[5] +"' \n");
					}
					else
					{
						sbQuery.Append(" AND S.登録日  BETWEEN '"+ sKey[4] + "' AND '"+ sKey[5] +"' \n");
					}
					
					if(sKey[6] != "00")
					{
						if(sKey[6] == "aa")
							sbQuery.Append(" AND S.状態 <> '01' \n");
// MOD 2009.05.11 東都）高木 未出荷対応 START
						else if(b未出荷){
							sbQuery.Append(" AND S.出荷日 < TO_CHAR(SYSDATE,'YYYYMMDD') \n");
							sbQuery.Append(" AND S.状態   = '02' \n");
						}
// MOD 2009.05.11 東都）高木 未出荷対応 END
//保留 MOD 2010.10.12 東都）高木 運賃エラー対応 START
//保留						else if(b運賃エラー){
//保留							sbQuery.Append(" AND ( S.運賃 > 0 OR S.中継 > 0 OR S.諸料金 > 0 ) \n");
//保留						}
//保留 MOD 2010.10.12 東都）高木 運賃エラー対応 END
						else
							sbQuery.Append(" AND S.状態 = '"+ sKey[6] + "' \n");
					}
				}
				else
				{
					sbQuery.Append(" WHERE S.送り状番号 = '"+ sKey[7] + "' \n");
				}
				if(sKey[8] != "0")
				{
					if(sKey[8] == "1")
					{
// MOD 2007.01.17 東都）高木 一覧項目に削除ＦＧ、ラベル印刷済区分を表示 START
//						sbQuery.Append(" AND S.削除ＦＧ <> '0' \n");
						sbQuery.Append(" AND S.削除ＦＧ = '1' \n");
// MOD 2007.01.17 東都）高木 一覧項目に削除ＦＧ、ラベル印刷済区分を表示 END
					}
					else
					{
						sbQuery.Append(" AND S.削除ＦＧ = '0' \n");
					}
				}

// MOD 2009.05.11 東都）高木 未出荷対応 START
				if(sKey[9].Length > 0){
// MOD 2009.05.11 東都）高木 未出荷対応 END
//保留 MOD 2010.07.21 東都）高木 リコー様対応 START
					sbQuery.Append(" AND S.発店ＣＤ = '"+ sKey[9] + "' \n");
//					sbQuery.Append(" AND ( S.発店ＣＤ = '"+ sKey[9] + "' OR S.登録者 = '"+ sKey[9] + "') \n");
//保留 MOD 2010.07.21 東都）高木 リコー様対応 END
// MOD 2009.05.11 東都）高木 未出荷対応 START
				}
// MOD 2009.05.11 東都）高木 未出荷対応 END
				sbQuery.Append(" AND S.状態     = J.状態ＣＤ(+) \n");
				sbQuery.Append(" AND S.詳細状態 = J.状態詳細ＣＤ(+) \n");
// MOD 2009.05.11 東都）高木 未出荷対応 START
				sbQuery.Append(" AND S.会員ＣＤ = CM01.会員ＣＤ(+) \n");
// MOD 2009.05.11 東都）高木 未出荷対応 END

				sbQuery2.Append(GET_SYUKKA_UNION_1);
// ADD 2007.01.11 東都）高木 送り状番号で検索した時、お客様名、依頼主名を表示 START
				//送り状番号が入力されている場合
				if(sKey[7].Length > 0)
				{
					sbQuery2.Append(", NVL(MIN(A.会員ＣＤ),' ') \n");
					sbQuery2.Append(", NVL(MIN(A.部門ＣＤ),' ') \n");
					sbQuery2.Append(", NVL(MIN(A.荷送人ＣＤ),' ') \n");
				}
				sbQuery2.Append(" FROM (  \n");
// ADD 2007.01.11 東都）高木 送り状番号で検索した時、お客様名、依頼主名を表示 END
				if(sKey[7].Length == 0)
				{
					sbQuery2.Append(GET_SYUKKA_INDEX_2);
				}
				else
				{
					sbQuery2.Append(GET_SYUKKA_INDEX_1);
				}
				sbQuery2.Append(GET_SYUKKA_SELECT_1);
// ADD 2007.01.11 東都）高木 送り状番号で検索した時、お客様名、依頼主名を表示 START
				//送り状番号が入力されている場合
				if(sKey[7].Length > 0)
				{
					sbQuery2.Append(", NVL(S.会員ＣＤ, ' ')   AS 会員ＣＤ \n");
					sbQuery2.Append(", NVL(S.部門ＣＤ, ' ')   AS 部門ＣＤ \n");
					sbQuery2.Append(", NVL(S.荷送人ＣＤ, ' ') AS 荷送人ＣＤ \n");
				}
// ADD 2007.01.11 東都）高木 送り状番号で検索した時、お客様名、依頼主名を表示 END
				sbQuery2.Append(GET_SYUKKA_FROM_1);
				sbQuery2.Append(sbQuery);
// MOD 2009.05.11 東都）高木 未出荷対応 START
//保留 MOD 2010.10.12 東都）高木 運賃エラー対応 START
				if(!b未出荷){
//保留				if(!b未出荷 && !b運賃エラー){
//保留 MOD 2010.10.12 東都）高木 運賃エラー対応 END
// MOD 2009.05.11 東都）高木 未出荷対応 END
					sbQuery2.Append(" UNION ALL \n");
					if(sKey[7].Length == 0)
					{
						sbQuery2.Append(GET_SYUKKA_INDEX_4);
					}
					else
					{
						sbQuery2.Append(GET_SYUKKA_INDEX_3);
					}
					sbQuery2.Append(GET_SYUKKA_SELECT_1);
// ADD 2007.01.11 東都）高木 送り状番号で検索した時、お客様名、依頼主名を表示 START
					//送り状番号が入力されている場合
					if(sKey[7].Length > 0)
					{
						sbQuery2.Append(", NVL(S.会員ＣＤ, ' ')   AS 会員ＣＤ \n");
						sbQuery2.Append(", NVL(S.部門ＣＤ, ' ')   AS 部門ＣＤ \n");
						sbQuery2.Append(", NVL(S.荷送人ＣＤ, ' ') AS 荷送人ＣＤ \n");
					}
// ADD 2007.01.11 東都）高木 送り状番号で検索した時、お客様名、依頼主名を表示 END
					sbQuery2.Append(GET_SYUKKA_FROM_2);
					sbQuery2.Append(sbQuery);
// MOD 2009.05.11 東都）高木 未出荷対応 START
				}
// MOD 2009.05.11 東都）高木 未出荷対応 END
				sbQuery2.Append(" ) A \n");

				OracleDataReader reader = CmdSelect(sUser, conn2, sbQuery2);

				if(reader.Read())
				{
					s登録件数   = reader.GetDecimal(0).ToString("#,##0").Trim();
					s個数合計   = reader.GetDecimal(1).ToString("#,##0").Trim();
					d重量合計   = reader.GetDecimal(2);
					d才数合計   = reader.GetDecimal(3);
// ADD 2007.01.11 東都）高木 送り状番号で検索した時、お客様名、依頼主名を表示 START
					//送り状番号が入力されている場合
					if(sKey[7].Length > 0)
					{
// MOD 2011.05.18 東都）高木 お客様ごとに重量入力制御 START
//						s会員ＣＤ   = reader.GetString(4).Trim();
//						s部門ＣＤ   = reader.GetString(5).Trim();
//						s荷送人ＣＤ = reader.GetString(6).Trim();
//// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 START
//						d運賃重量計   = reader.GetDecimal(7);
//						d運賃才数計   = reader.GetDecimal(8);
						d運賃重量計   = reader.GetDecimal(4);
						d運賃才数計   = reader.GetDecimal(5);
						s会員ＣＤ   = reader.GetString(6).Trim();
						s部門ＣＤ   = reader.GetString(7).Trim();
						s荷送人ＣＤ = reader.GetString(8).Trim();
// MOD 2011.05.18 東都）高木 お客様ごとに重量入力制御 END
					}else{
						d運賃重量計   = reader.GetDecimal(4);
						d運賃才数計   = reader.GetDecimal(5);
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 END
					}
// ADD 2007.01.11 東都）高木 送り状番号で検索した時、お客様名、依頼主名を表示 END
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END

				sRet[1] = s登録件数;
				sRet[2] = s個数合計;
				d重量合計 = d重量合計 + d才数合計 * 8;
				sRet[3] = d重量合計.ToString("#,##0").Trim();
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 START
				if(sUser.Length > 3 && sUser[3].Length >= 4){ // 2.10以降（臨時対応）
					d運賃重量計 = d運賃重量計 + d運賃才数計 * 8;
					sRet[3] += "|" + d運賃重量計.ToString("#,##0").Trim();
				}
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 END
// ADD 2007.01.11 東都）高木 送り状番号で検索した時、お客様名、依頼主名を表示 START
				sRet[4] = s会員ＣＤ;
				sRet[5] = s部門ＣＤ;
				sRet[6] = s荷送人ＣＤ;
// ADD 2007.01.11 東都）高木 送り状番号で検索した時、お客様名、依頼主名を表示 END

				i登録件数 = int.Parse(s登録件数.Replace(",",""));

				if(i登録件数 == 0)
				{
					sRet[0] = "該当データがありません";
				}
// MOD 2007.01.23 東都）高木 1000件以上は表示できなくする START
//				else if(i登録件数 > 5000)
//				{
//					sRet[0] = "5000件オーバー";
//					logWriter(sUser, INF, sRet[0]);
//					return sRet;
//				}
// MOD 2011.05.20 東都）高木 出荷照会件数の検索上限解除 START
//				else if(i登録件数 > 1000)
				else if(i登録件数 > 1000 && b検索上限解除 == false)
// MOD 2011.05.20 東都）高木 出荷照会件数の検索上限解除 END
				{
					sRet[0] = "1000件オーバー";
					logWriter(sUser, INF, sRet[0]);
					return sRet;
				}
// MOD 2007.01.23 東都）高木 1000件以上は表示できなくする END
				else
				{
// MOD 2007.01.11 東都）高木 送り状番号で検索した時、お客様名、依頼主名を表示 START
//					sRet = new string[i登録件数 + 4];
					sRet = new string[i登録件数 + 10];
// MOD 2007.01.11 東都）高木 送り状番号で検索した時、お客様名、依頼主名を表示 END
					sRet[0] = "正常終了";
					sRet[1] = s登録件数;
					sRet[2] = s個数合計;
					sRet[3] = d重量合計.ToString("#,##0").Trim();
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 START
					if(sUser.Length > 3 && sUser[3].Length >= 4){ // 2.10以降（臨時対応）
						sRet[3] += "|" + d運賃重量計.ToString("#,##0").Trim();
					}
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 END
// ADD 2007.01.11 東都）高木 送り状番号で検索した時、お客様名、依頼主名を表示 START
					sRet[4] = s会員ＣＤ;
					sRet[5] = s部門ＣＤ;
					sRet[6] = s荷送人ＣＤ;
					string[] sIRet = Get_Sirainusi(sUser, s会員ＣＤ, s部門ＣＤ, s荷送人ＣＤ, sKey[9]);
					if(sIRet[0].Length == 4){
						sRet[7] = sIRet[1];
						sRet[8] = sIRet[2];
						sRet[9] = sIRet[3];
					}
// ADD 2007.01.11 東都）高木 送り状番号で検索した時、お客様名、依頼主名を表示 END

					sbQuery2 = new StringBuilder(1024);
					if(sKey[3] == "0")
					{
						sbQuery2.Append(" SELECT * FROM ( \n");
						if(sKey[7].Length == 0)
						{
							sbQuery2.Append(GET_SYUKKA_INDEX_2);
						}
						else
						{
							sbQuery2.Append(GET_SYUKKA_INDEX_1);
						}
						sbQuery2.Append(GET_SYUKKA_SELECT_2);
// MOD 2011.05.20 東都）高木 出荷照会件数の検索上限解除 START
						if(b検索上限解除){
							sbQuery2.Append(GET_SYUKKA_SELECT_22);
						}
// MOD 2011.05.20 東都）高木 出荷照会件数の検索上限解除 END
						sbQuery2.Append(GET_SYUKKA_FROM_1);
						sbQuery2.Append(sbQuery);
// MOD 2009.05.11 東都）高木 未出荷対応 START
//保留 MOD 2010.10.12 東都）高木 運賃エラー対応 END
						if(!b未出荷){
//保留						if(!b未出荷 && !b運賃エラー){
//保留 MOD 2010.10.12 東都）高木 運賃エラー対応 END
// MOD 2009.05.11 東都）高木 未出荷対応 END
							sbQuery2.Append(" UNION ALL \n");
							if(sKey[7].Length == 0)
							{
								sbQuery2.Append(GET_SYUKKA_INDEX_4);
							}
							else
							{
								sbQuery2.Append(GET_SYUKKA_INDEX_3);
							}
							sbQuery2.Append(GET_SYUKKA_SELECT_2);
// MOD 2011.05.20 東都）高木 出荷照会件数の検索上限解除 START
							if(b検索上限解除){
								sbQuery2.Append(GET_SYUKKA_SELECT_22);
							}
// MOD 2011.05.20 東都）高木 出荷照会件数の検索上限解除 END
							sbQuery2.Append(GET_SYUKKA_FROM_2);
							sbQuery2.Append(sbQuery);
// MOD 2009.05.11 東都）高木 未出荷対応 START
						}
// MOD 2009.05.11 東都）高木 未出荷対応 END
						sbQuery2.Append(" ) A \n");
						sbQuery2.Append(GET_SYUKKA_SELECT_2_SORT2);
						reader = CmdSelect(sUser, conn2, sbQuery2);
					}
					else
					{
						sbQuery2.Append(" SELECT * FROM ( \n");
						if(sKey[7].Length == 0)
						{
							sbQuery2.Append(GET_SYUKKA_INDEX_2);
						}
						else
						{
							sbQuery2.Append(GET_SYUKKA_INDEX_1);
						}
						sbQuery2.Append(GET_SYUKKA_SELECT_2);
// MOD 2011.05.20 東都）高木 出荷照会件数の検索上限解除 START
						if(b検索上限解除){
							sbQuery2.Append(GET_SYUKKA_SELECT_22);
						}
// MOD 2011.05.20 東都）高木 出荷照会件数の検索上限解除 END
						sbQuery2.Append(GET_SYUKKA_FROM_1);
						sbQuery2.Append(sbQuery);
// MOD 2009.05.11 東都）高木 未出荷対応 START
//保留 MOD 2010.10.12 東都）高木 運賃エラー対応 START
						if(!b未出荷){
//保留						if(!b未出荷 && !b運賃エラー){
//保留 MOD 2010.10.12 東都）高木 運賃エラー対応 END
// MOD 2009.05.11 東都）高木 未出荷対応 END
							sbQuery2.Append(" UNION ALL \n");
							if(sKey[7].Length == 0)
							{
								sbQuery2.Append(GET_SYUKKA_INDEX_4);
							}
							else
							{
								sbQuery2.Append(GET_SYUKKA_INDEX_3);
							}
							sbQuery2.Append(GET_SYUKKA_SELECT_2);
// MOD 2011.05.20 東都）高木 出荷照会件数の検索上限解除 START
							if(b検索上限解除){
								sbQuery2.Append(GET_SYUKKA_SELECT_22);
							}
// MOD 2011.05.20 東都）高木 出荷照会件数の検索上限解除 END
							sbQuery2.Append(GET_SYUKKA_FROM_2);
							sbQuery2.Append(sbQuery);
// MOD 2009.05.11 東都）高木 未出荷対応 START
						}
// MOD 2009.05.11 東都）高木 未出荷対応 END
						sbQuery2.Append(" ) A \n");
						sbQuery2.Append(GET_SYUKKA_SELECT_2_SORT);
						reader = CmdSelect(sUser, conn2, sbQuery2);
					}

// MOD 2007.01.11 東都）高木 送り状番号で検索した時、お客様名、依頼主名を表示 START
//					int iCnt = 4;
					int iCnt = 10;
// MOD 2007.01.11 東都）高木 送り状番号で検索した時、お客様名、依頼主名を表示 END
					while (reader.Read() && iCnt < sRet.Length)
					{
						sbRet = new StringBuilder(1024);

// ADD 2007.01.17 東都）高木 一覧項目に削除ＦＧ、送り状発行済ＦＧを表示 START
						sbRet.Append(sSepa + reader.GetString(20));			// 削除ＦＧ
						sbRet.Append(sSepa + reader.GetString(21));			// 送り状発行済ＦＧ
// ADD 2007.01.17 東都）高木 一覧項目に削除ＦＧ、送り状発行済ＦＧを表示 END
						sbRet.Append(sSepa + reader.GetString(0));			// 出荷日
// MOD 2009.05.11 東都）高木 未出荷対応 START
//保留 MOD 2010.10.12 東都）高木 運賃エラー対応 START
						if(sKey[7].Length == 0 && b未出荷){
//保留						if(sKey[7].Length == 0 && ( b未出荷 || b運賃エラー )){
//保留 MOD 2010.10.12 東都）高木 運賃エラー対応 END
							sbRet.Append(sSepa + reader.GetString(25).Trim());	// 会員ＣＤ
							sbRet.Append(sCRLF + reader.GetString(26).Trim());	// 会員名
						}else{
// MOD 2009.05.11 東都）高木 未出荷対応 END
							sbRet.Append(sSepa + reader.GetString(1).Trim());	// 住所１
							sbRet.Append(sCRLF + reader.GetString(2).Trim());	// 名前１
// MOD 2011.05.20 東都）高木 出荷照会件数の検索上限解除 START
							if(b検索上限解除){
// MOD 2014.03.19 BEVAS）高杉 出荷照会に配完日付・時刻を追加 START
//								sbRet.Append(sCRLF + reader.GetString(49).Trim()); // 名前２
								sbRet.Append(sCRLF + reader.GetString(50).Trim()); // 名前２
// MOD 2014.03.19 BEVAS）高杉 出荷照会に配完日付・時刻を追加 END
							}
// MOD 2011.05.20 東都）高木 出荷照会件数の検索上限解除 END
// MOD 2009.05.11 東都）高木 未出荷対応 START
						}
// MOD 2009.05.11 東都）高木 未出荷対応 END
						sbRet.Append(sSepa + reader.GetString(3));			// 個数
// MOD 2011.04.13 東都）高木 重量入力不可対応 START
//						d才数合計 = reader.GetDecimal(17);
//						d才数合計 = d才数合計 * 8;
//						if(d才数合計 == 0)
//							sbRet.Append(sSepa + reader.GetDecimal(4).ToString("#,##0").Trim()); // 重量
//						else
//							sbRet.Append(sSepa + d才数合計.ToString("#,##0").Trim());		// 才数
						// お客様入力値
						d才数合計 = reader.GetDecimal(17) * 8;
						d才数合計 += reader.GetDecimal(4);
						if(d才数合計 == 0){
//							sbRet.Append(sSepa + " ");
							sbRet.Append(sSepa + "0");
						}else{
							sbRet.Append(sSepa + d才数合計.ToString("#,##0").Trim());
						}
						if(sUser.Length > 3 && sUser[3].Length >= 4){ // 2.10以降（臨時対応）
							s運賃才数 = reader.GetString(46).TrimEnd();
							s運賃重量 = reader.GetString(47).TrimEnd();
							d才数重量 = 0;
							if(s運賃才数.Length > 0){
								try{
									d才数重量 += (Decimal.Parse(s運賃才数) * 8);
								}catch(Exception){}
							}
							if(s運賃重量.Length > 0){
								try{
									d才数重量 += Decimal.Parse(s運賃重量);
								}catch(Exception){}
							}
							if(d才数重量 == 0){
								sbRet.Append(sSepa + " ");
							}else{
								sbRet.Append(sSepa + d才数重量.ToString("#,##0").Trim());
							}
						}
// MOD 2011.04.13 東都）高木 重量入力不可対応 END
// MOD 2007.01.17 東都）高木 一覧項目に削除ＦＧ、送り状発行済ＦＧを表示 START
//						sbRet.Append(sSepa + reader.GetDecimal(18).ToString("#,##0").Trim());
//																			// 保険料
//						sbRet.Append(sSepa + reader.GetDecimal(19).ToString("#,##0").Trim());
//																			// 運賃
//
//						sbRet.Append(sSepa + reader.GetString(5).TrimEnd());		// 輸送指示１
//						sbRet.Append(sCRLF + reader.GetString(6).Trim());		// 品名記事１
//						s送り状 = reader.GetString(7).Trim();              		// 送り状番号
//						if(s送り状.Length == 0)
//							sbRet.Append(sSepa + s送り状);
//						else
//							sbRet.Append(sSepa + s送り状.Remove(0,4));
//						sbRet.Append(sCRLF + reader.GetString(8));			// 元着区分
//						sbRet.Append(sSepa + reader.GetString(9));			// 指定日
//						sbRet.Append(sSepa + reader.GetString(10).Trim());	// 状態
//						sbRet.Append(sSepa + reader.GetString(11));			// 登録日
//						sbRet.Append(sSepa + reader.GetString(12).Trim());	// お客様出荷番号
						s送り状 = reader.GetString(7).Trim();              		// 送り状番号
						if(s送り状.Length == 0)
							sbRet.Append(sSepa + s送り状);
						else
							sbRet.Append(sSepa + s送り状.Remove(0,4));
						sbRet.Append(sCRLF + reader.GetString(8));			// 元着区分
// ADD 2007.11.22 東都）高木 一覧項目に発店ＣＤを表示 START
						sbRet.Append("　" + reader.GetString(22));			// 発店ＣＤ
// ADD 2007.11.22 東都）高木 一覧項目に発店ＣＤを表示 END
						sbRet.Append(sSepa + reader.GetString(12).Trim());	// お客様出荷番号
						sbRet.Append(sSepa + reader.GetString(9));			// 指定日
						sbRet.Append(sSepa + reader.GetString(10).Trim());	// 状態
// MOD 2014.03.19 BEVAS）高杉 出荷照会に配完日付・時刻を追加 START
						sbRet.Append(sSepa + reader.GetString(49).Trim());	// 配完日付・時刻
// MOD 2014.03.19 BEVAS）高杉 出荷照会に配完日付・時刻を追加 END
						sbRet.Append(sSepa + reader.GetString(5).TrimEnd());// 輸送指示１
						sbRet.Append(sCRLF + reader.GetString(6).Trim());	// 品名記事１
// MOD 2011.05.20 東都）高木 出荷照会件数の検索上限解除 START
						if(b検索上限解除){
// MOD 2014.03.19 BEVAS）高杉 出荷照会に配完日付・時刻を追加 START
//							sbRet.Append(sCRLF + reader.GetString(50).Trim()); // 品名記事２
//							sbRet.Append(sCRLF + reader.GetString(51).Trim()); // 品名記事３
//							sbRet.Append(sCRLF + reader.GetString(52).Trim()); // 品名記事４
//							sbRet.Append(sCRLF + reader.GetString(53).Trim()); // 品名記事５
//							sbRet.Append(sCRLF + reader.GetString(54).Trim()); // 品名記事６
							sbRet.Append(sCRLF + reader.GetString(51).Trim()); // 品名記事２
							sbRet.Append(sCRLF + reader.GetString(52).Trim()); // 品名記事３
							sbRet.Append(sCRLF + reader.GetString(53).Trim()); // 品名記事４
							sbRet.Append(sCRLF + reader.GetString(54).Trim()); // 品名記事５
							sbRet.Append(sCRLF + reader.GetString(55).Trim()); // 品名記事６
// MOD 2014.03.19 BEVAS）高杉 出荷照会に配完日付・時刻を追加 END
// MOD 2011.07.28 東都）高木 記事行の追加 END
						}
// MOD 2011.05.20 東都）高木 出荷照会件数の検索上限解除 END
						sbRet.Append(sSepa + reader.GetDecimal(19).ToString("#,##0").Trim());
																			// 運賃
						sbRet.Append(sSepa + reader.GetDecimal(18).ToString("#,##0").Trim());
																			// 保険料
						sbRet.Append(sSepa + reader.GetString(11));			// 登録日
// MOD 2007.01.17 東都）高木 一覧項目に削除ＦＧ、送り状発行済ＦＧを表示 END
						sbRet.Append(sSepa + reader.GetString(13));			// ジャーナルＮＯ
						sbRet.Append(sSepa + reader.GetString(14));			// 登録日
						sbRet.Append(sSepa + reader.GetString(15));			// 出荷日
						sbRet.Append(sSepa + reader.GetString(16));			// 登録者
// MOD 2009.09.11 東都）高木 出荷照会で出荷済ＦＧ,送信済ＦＧなどを追加 START
						sbRet.Append(sSepa + reader.GetString(28));			// 出荷済ＦＧ
						sbRet.Append(sSepa + reader.GetString(29));			// 送信済ＦＧ
						sbRet.Append(sSepa + reader.GetDecimal(30).ToString());
																			// 登録日時
						sbRet.Append(sSepa + reader.GetDecimal(31).ToString());
																			// 更新日時
						sbRet.Append(sSepa + reader.GetString(32));			// 更新ＰＧ
						sbRet.Append(sSepa + reader.GetString(33));			// 更新者
// MOD 2009.09.11 東都）高木 出荷照会で出荷済ＦＧ,送信済ＦＧなどを追加 END
// MOD 2010.04.06 東都）高木 出荷照会に得意先、郵便番号などを追加 START
						sbRet.Append(sSepa + reader.GetString(34).TrimEnd()); // 得意先ＣＤ
						sbRet.Append(sSepa + reader.GetString(35).TrimEnd()); // 部課ＣＤ
						sbRet.Append(sSepa + reader.GetString(36).TrimEnd()); // 部課名

						sbRet.Append(sSepa + reader.GetString(37).TrimEnd()); // 郵便番号
						sbRet.Append(sSepa + reader.GetString(38).TrimEnd()); // 着店ＣＤ
						sbRet.Append(sSepa + reader.GetString(39).TrimEnd()); // 着店名

						sbRet.Append(sSepa + reader.GetString(25).TrimEnd()); // 会員ＣＤ
						sbRet.Append(sSepa + reader.GetString(27).TrimEnd()); // 部門ＣＤ
						sbRet.Append(sSepa + reader.GetString(40).TrimEnd()); // 荷送人ＣＤ
// MOD 2010.04.06 東都）高木 出荷照会に得意先、郵便番号などを追加 END
// MOD 2010.10.12 東都）高木 運賃エラー対応 START
						sbRet.Append(sSepa + reader.GetString(42).TrimEnd()); // 運賃エラー確認ＦＧ
// MOD 2010.10.12 東都）高木 運賃エラー対応 END
// MOD 2010.11.25 東都）高木 出荷照会に削除日時などを追加 START
						sbRet.Append(sSepa + reader.GetDecimal(43).ToString());
																			// 削除日時
						sbRet.Append(sSepa + reader.GetString(44));			// 削除ＰＧ
						sbRet.Append(sSepa + reader.GetString(45));			// 削除者
// MOD 2010.11.25 東都）高木 出荷照会に削除日時などを追加 END
						sbRet.Append(sSepa);
						sRet[iCnt] = sbRet.ToString();
						iCnt++;
					}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
					disposeReader(reader);
					reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
				}

				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			
			return sRet;
		}
// ADD 2006.12.18 東都）小童谷 出荷一覧取得 END
// ADD 2006.12.20 東都）高木 稼動率調査表の機能追加 START
		/*********************************************************************
		 * 稼動率調査表ＣＳＶ出力用２
		 * 引数：会員情報、地区（開始、終了）、店所（開始、終了）、
		 *		 日付（開始、終了）
		 * 戻値：ステータス、店所名、得意先部課名、...
		 *
		 *********************************************************************/
		private static string GET_KADOURITU_CSV2_SELECT1
			= ""
			+ "SELECT * FROM ( \n"
			+ " SELECT \n"
			+ "  MAX(地区１) AS 地区１ \n"
			+ ", MAX(地区２) AS 地区２ \n"
			+ ", 店所ＣＤ \n"
			+ ", MAX(店所名)   AS 店所名 \n"
// MOD 2007.08.29 東都）高木 稼働率集計の変更 START
//			+ ", MAX(導入台数) AS 導入台数 \n"
//			+ ", MAX(稼動台数) AS 稼動台数 \n"
			+ ", SUM(導入台数) AS 導入台数 \n"
			+ ", SUM(稼動台数) AS 稼動台数 \n"
// MOD 2007.08.29 東都）高木 稼働率集計の変更 END
			+ " FROM ＳＴ０３稼働率 \n"
			;
// ADD 2007.10.11 東都）高木 デモ機を除く機能の追加 START
		private static string GET_KADOURITU_CSV2_SELECT1_2
			= ""
			+ "SELECT * FROM ( \n"
			+ " SELECT \n"
			+ "  MAX(地区１) AS 地区１ \n"
			+ ", MAX(地区２) AS 地区２ \n"
			+ ", 店所ＣＤ \n"
			+ ", MAX(店所名)   AS 店所名 \n"
			+ ", SUM(DECODE(集計区分,'D',稼動台数,導入台数)) AS 導入台数 \n"
			+ ", SUM(稼動台数) AS 稼動台数 \n"
			+ " FROM ＳＴ０３稼働率 \n"
			;
// ADD 2007.10.11 東都）高木 デモ機を除く機能の追加 END
		private static string GET_KADOURITU_CSV2_SELECT2
			= ""
			+ " GROUP BY 店所ＣＤ \n"
			+ " ) \n"
			+ " WHERE ( 導入台数 > 0 OR 稼動台数 > 0 OR 地区１ > ' ' OR 地区２ > ' ' ) \n"
			;
		private static string GET_KADOURITU_CSV2_ORDER
			= ""
			+ " ORDER BY 地区１, 地区２, 店所ＣＤ \n"
			;

		[WebMethod]
		public String[] Get_kadouritu_csv2(string[] sUser, string[] sData)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "稼動率調査表ＣＳＶ出力用取得２開始");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();

			string[] sRet = new string[1];
			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			StringBuilder sbQuery = new StringBuilder(1024);
			try
			{
// MOD 2007.10.11 東都）高木 デモ機を除く機能の追加 START
//				sbQuery.Append(GET_KADOURITU_CSV2_SELECT1);
				if(sData.Length >= 7 && sData[6] == "1"){
					sbQuery.Append(GET_KADOURITU_CSV2_SELECT1_2);
				}else{
					sbQuery.Append(GET_KADOURITU_CSV2_SELECT1);
				}
// MOD 2007.10.11 東都）高木 デモ機を除く機能の追加 END
				sbQuery.Append("WHERE 出荷日開始 = '"+ sData[4] + "' \n");
// MOD 2007.02.01 東都）高木 稼動率調査表の条件変更 START
//				sbQuery.Append("  AND 出荷日終了 = '"+ sData[5] + "' \n");
				if(sData[5] != null && sData[5].Trim().Length > 0){
					sbQuery.Append("  AND 出荷日終了 = '"+ sData[5] + "' \n");
				}
// MOD 2007.02.01 東都）高木 稼動率調査表の条件変更 END
// MOD 2007.10.11 東都）高木 デモ機を除く機能の追加 START
				if(sData.Length >= 7 && sData[6] == "1"){
// MOD 2016.04.08 bevas) 松本 社内伝票機能追加対応 START
					//王子運送側の導入関連集計も条件に追加（集計区分：「F」）
//					sbQuery.Append("  AND 集計区分 <= 'D' \n");
					sbQuery.Append("  AND 集計区分 in ('A', 'B', 'C', 'D', 'F') \n");
// MOD 2016.04.08 bevas) 松本 社内伝票機能追加対応 END
				}
// MOD 2007.10.11 東都）高木 デモ機を除く機能の追加 END

// DEL 2007.02.01 東都）高木 稼動率調査表の条件変更 START
//				//地区
//				if(sData[0].Length > 0)
//				{
//					if(sData[1].Length > 0)
//					{
//						sbQuery.Append(" AND 地区１  BETWEEN '"+ sData[0] + "' AND '"+ sData[1] +"' \n");
//					}
//					else
//					{
//						sbQuery.Append(" AND 地区１ = '"+ sData[0] + "' \n");
//					}
//				}
// DEL 2007.02.01 東都）高木 稼動率調査表の条件変更 START

				//店所
				if(sData[2].Length > 0)
				{
					if(sData[3].Length > 0)
					{
						sbQuery.Append(" AND 店所ＣＤ  BETWEEN '"+ sData[2] + "' AND '"+ sData[3] +"' \n");
					}
					else
					{
						sbQuery.Append(" AND 店所ＣＤ = '"+ sData[2] + "' \n");
					}
				}

				sbQuery.Append(GET_KADOURITU_CSV2_SELECT2);

// ADD 2007.02.01 東都）高木 稼動率調査表の条件変更 START
				//地区
				if(sData[0].Length > 0)
				{
					if(sData[1].Length > 0)
					{
						sbQuery.Append(" AND 地区１  BETWEEN '"+ sData[0] + "' AND '"+ sData[1] +"' \n");
					}
					else
					{
						sbQuery.Append(" AND 地区１ = '"+ sData[0] + "' \n");
					}
				}
// ADD 2007.02.01 東都）高木 稼動率調査表の条件変更 START

				sbQuery.Append(GET_KADOURITU_CSV2_ORDER);

				OracleDataReader reader = CmdSelect(sUser, conn2, sbQuery);;

				StringBuilder sbData = new StringBuilder(1024);
				while (reader.Read())
				{
					sbData = new StringBuilder(1024);
					sbData.Append(         sDbl + sSng + reader.GetString(0).Trim() + sDbl);	// 地区１
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(1).Trim() + sDbl);	// 地区２
//保留：不要な項目は削除できるか？
					sbData.Append(sKanma + sDbl + sDbl);										// 地区１名称
					sbData.Append(sKanma + sDbl + sDbl);										// 地区２名称
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(2).Trim() + sDbl);	// 店所ＣＤ
					sbData.Append(sKanma + sDbl        + reader.GetString(3).Trim() + sDbl);	// 店所名
					double d導入台数 = double.Parse(reader.GetDecimal(4).ToString().Trim());	// 導入台数
					double d稼動台数 = double.Parse(reader.GetDecimal(5).ToString().Trim());	// 稼動台数
					sbData.Append(sKanma + sDbl        + d導入台数 + sDbl);
					sbData.Append(sKanma + sDbl        + d稼動台数 + sDbl);
//保留：計算はＥＸＣＥＬに移動できるか？
					if(d導入台数 == 0 || d稼動台数 == 0){
						sbData.Append(sKanma + sDbl + "0.0" + sDbl);
					}else{
						sbData.Append(sKanma + sDbl + (d稼動台数 * 100 / d導入台数).ToString("00.0") + sDbl);
					}

					sList.Add(sbData);
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "該当データがありません";
				else
				{
					sRet[0] = "正常終了";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}
// ADD 2006.12.20 東都）高木 稼動率調査表の機能追加 END
// ADD 2006.12.20 東都）高木 出荷状況一覧表の機能追加 START
		/*********************************************************************
		 * 出荷状況一覧表（ＣＳＶ出力用）２
		 * 引数：会員情報、地区（開始、終了）、店所（開始、終了）、
		 *		 出荷日（開始、終了）
		 * 戻値：ステータス、店所名、得意先部課名、...
		 *
		 *********************************************************************/
// MOD 2007.02.01 東都）高木 出荷状況一覧表の変更 START
//		private static string GET_SYUUKA_INF2_SELECT
//			= "SELECT NVL(CM10.地区１,' ') AS 地区１ \n"
//			+      ", ST04.店所ＣＤ \n"
//			+      ", NVL(CM10.店所名, ST04.店所ＣＤ) AS 店所名 \n"
//			+      ", ST04.荷送人ＣＤ \n"
//			+      ", SM01.名前１ \n"
//			+      ", SM01.名前２ \n"
//			+      ", ST04.得意先ＣＤ  \n"
//			+      ", ST04.部課ＣＤ \n"
//			+      ", ST04.部課名 \n"
//			+      ", ST04.出荷日 \n"
//			+      ", ST04.件数 \n"
//			+      ", ST04.個数 \n"
//			+      ", ST04.重量 + (ST04.才数 * 8) AS 重量 \n"
////保留：未使用？
////			+      ", ST04.福通重量 + (ST04.福通才数 * 8) AS 福通重量 \n"
//			+      ", 0 AS 福通重量 \n"
//			+      ", ST04.運賃 AS 運賃 \n"
//			+      ", ST04.中継 AS 中継費 \n"
//			+      ", ST04.保険金額  AS 保険料 \n"
//			+      " FROM \n"
//			+      "  ＳＴ０４出荷状況 ST04 \n"
//			+      ", ＣＭ１０店所     CM10 \n"
//			+      ", ＳＭ０１荷送人   SM01 \n"
//			;
		private static string GET_SYUUKA_INF2_SELECT
			= "SELECT NVL(CM10.地区１,' ') AS 地区１ \n"
			+      ", NVL(CM10.地区２,' ') AS 地区２ \n"
			+      ", ST04W.店所ＣＤ \n"
			+      ", NVL(CM10.店所名,' ') \n"
			+      ", ST04W.会員ＣＤ \n"
			+      ", NVL(CM01.会員名,' ') \n"
			+      ", ST04W.部門ＣＤ \n"
			+      ", NVL(CM02.部門名,' ') \n"
			+      ", ST04W.件数 \n"
			+      ", ST04W.個数 \n"
			+      ", ST04W.重量 + (ST04W.才数 * 8) \n"
			+      ", ST04W.運賃 \n"
			+      ", ST04W.中継 \n"
			+      ", ST04W.保険金額 \n"
			+      " FROM \n"
			+      "(SELECT \n"
			+      "  ST04.店所ＣＤ \n"
			+      ", ST04.会員ＣＤ \n"
			+      ", ST04.部門ＣＤ \n"
			+      ", SUM(ST04.件数)     AS 件数 \n"
			+      ", SUM(ST04.個数)     AS 個数 \n"
// MOD 2011.04.13 東都）高木 重量入力不可対応 START
//			+      ", SUM(ST04.重量)     AS 重量 \n"
//			+      ", SUM(ST04.才数)     AS 才数 \n"
			+      ", SUM(ST04.福通重量) AS 重量 \n"
			+      ", SUM(ST04.福通才数) AS 才数 \n"
// MOD 2011.04.13 東都）高木 重量入力不可対応 END
			+      ", SUM(ST04.運賃)     AS 運賃 \n"
			+      ", SUM(ST04.中継)     AS 中継 \n"
			+      ", SUM(ST04.保険金額) AS 保険金額 \n"
			+      "  FROM ＳＴ０４出荷状況 ST04 \n"
			;

		private static string GET_SYUUKA_INF2_FROM
			=      ") ST04W \n"
			+      ", ＣＭ１０店所     CM10 \n"
			+      ", ＣＭ０１会員     CM01 \n"
			+      ", ＣＭ０２部門     CM02 \n"
			;
// MOD 2007.02.01 東都）高木 出荷状況一覧表の変更 END

		[WebMethod]
		public String[] Get_syuuka_Inf2(string[] sUser, string[] sData)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "出荷状況一覧表出力用取得２開始");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();

			string[] sRet = new string[1];
			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			StringBuilder sbQuery  = new StringBuilder(1024);
			try
			{
				sbQuery.Append(GET_SYUUKA_INF2_SELECT);
				sbQuery.Append(" WHERE ST04.出荷日 BETWEEN '"+ sData[4] + "' AND '"+ sData[5] +"' \n");

// ADD 2007.02.01 東都）高木 出荷状況一覧表の変更 START
				//店所
				if(sData[2].Length > 0)
				{
					if(sData[3].Length > 0)
					{
						sbQuery.Append(" AND ST04.店所ＣＤ  BETWEEN '"+ sData[2] + "' AND '"+ sData[3] +"' \n");
					}
					else
					{
						sbQuery.Append(" AND ST04.店所ＣＤ = '"+ sData[2] + "' \n");
					}
				}
				sbQuery.Append(" GROUP BY ST04.店所ＣＤ, ST04.会員ＣＤ, ST04.部門ＣＤ \n");
				sbQuery.Append(GET_SYUUKA_INF2_FROM);
// ADD 2007.02.01 東都）高木 出荷状況一覧表の変更 END

				//地区
				if(sData[0].Length > 0)
				{
// MOD 2007.02.01 東都）高木 出荷状況一覧表の変更 START
//					sbQuery.Append(" AND ST04.店所ＣＤ = CM10.店所ＣＤ \n");
					sbQuery.Append(" WHERE ST04W.店所ＣＤ = CM10.店所ＣＤ \n");
// MOD 2007.02.01 東都）高木 出荷状況一覧表の変更 END
					if(sData[1].Length > 0)
					{
						sbQuery.Append(" AND CM10.地区１ BETWEEN '"+ sData[0] + "' AND '"+ sData[1] +"' \n");
					}
					else
					{
						sbQuery.Append(" AND CM10.地区１ = '"+ sData[0] + "' \n");
					}
				}
				else
				{
// MOD 2007.02.01 東都）高木 出荷状況一覧表の変更 START
//					sbQuery.Append(" AND ST04.店所ＣＤ = CM10.店所ＣＤ(+) \n");
					sbQuery.Append(" WHERE ST04W.店所ＣＤ = CM10.店所ＣＤ(+) \n");
// MOD 2007.02.01 東都）高木 出荷状況一覧表の変更 END
				}

// DEL 2007.02.01 東都）高木 出荷状況一覧表の変更 START
//				//店所
//				if(sData[2].Length > 0)
//				{
//					if(sData[3].Length > 0)
//					{
//						sbQuery.Append(" AND ST04.店所ＣＤ  BETWEEN '"+ sData[2] + "' AND '"+ sData[3] +"' \n");
//					}
//					else
//					{
//						sbQuery.Append(" AND ST04.店所ＣＤ = '"+ sData[2] + "' \n");
//					}
//				}
// DEL 2007.02.01 東都）高木 出荷状況一覧表の変更 END

// MOD 2007.02.01 東都）高木 出荷状況一覧表の変更 START
//				// ＳＭ０１荷送人
//				sbQuery.Append(" AND ST04.会員ＣＤ   = SM01.会員ＣＤ \n");
//				sbQuery.Append(" AND ST04.部門ＣＤ   = SM01.部門ＣＤ \n");
//				sbQuery.Append(" AND ST04.荷送人ＣＤ = SM01.荷送人ＣＤ \n");
//
//				sbQuery.Append(" ORDER BY 出荷日, 地区１, 店所ＣＤ, 得意先ＣＤ, 部課ＣＤ \n");
				// ＣＭ０１会員
				sbQuery.Append(" AND ST04W.会員ＣＤ = CM01.会員ＣＤ(+) \n");
				// ＣＭ０２部門
				sbQuery.Append(" AND ST04W.会員ＣＤ = CM02.会員ＣＤ(+) \n");
				sbQuery.Append(" AND ST04W.部門ＣＤ = CM02.部門ＣＤ(+) \n");

// MOD 2007.02.06 東都）高木 桑田殿テストエラー修正 START
//				sbQuery.Append(" ORDER BY 地区１, 地区２, ST04W.店所ＣＤ, ST04W.会員ＣＤ, ST04W.部門ＣＤ \n");
				sbQuery.Append(" ORDER BY 地区１, 地区２, ST04W.店所ＣＤ, ST04W.部門ＣＤ, ST04W.会員ＣＤ \n");
// MOD 2007.02.06 東都）高木 桑田殿テストエラー修正 END
// MOD 2007.02.01 東都）高木 出荷状況一覧表の変更 END

				OracleDataReader reader = CmdSelect(sUser, conn2, sbQuery);

				StringBuilder sbData = new StringBuilder(1024);
				while (reader.Read())
				{
					sbData = new StringBuilder(1024);
// MOD 2007.02.01 東都）高木 出荷状況一覧表の変更 START
//					sbData.Append(         sDbl + sSng + reader.GetString(0).Trim()              + sDbl);	// 地区ＣＤ
//					sbData.Append(sKanma + sDbl + sSng + reader.GetString(1).Trim()              + sDbl);	// 店所ＣＤ
//					sbData.Append(sKanma + sDbl +        reader.GetString(2).Trim()              + sDbl);	// 店所名
//					sbData.Append(sKanma + sDbl +        reader.GetString(4).Trim() + reader.GetString(5).Trim() + sDbl);
//																											// 荷主名
//					sbData.Append(sKanma + sDbl + sSng + reader.GetString(3).Trim()              + sDbl);	// 荷主名ＣＤ
//					sbData.Append(sKanma + sDbl + sSng + reader.GetString(9).Trim()              + sDbl);	// 出荷日
//					sbData.Append(sKanma + sDbl +        reader.GetDecimal(10).ToString().Trim() + sDbl);	// データ件数
//					sbData.Append(sKanma + sDbl +        reader.GetDecimal(11).ToString().Trim() + sDbl);	// 個数
//					sbData.Append(sKanma + sDbl +        reader.GetDecimal(12).ToString().Trim() + sDbl);	// 重量
//																											// 
//					sbData.Append(sKanma + sDbl +        reader.GetDecimal(14).ToString().Trim() + sDbl);	// 運賃
//					sbData.Append(sKanma + sDbl +        reader.GetDecimal(15).ToString().Trim() + sDbl);	// 中継費
//					sbData.Append(sKanma + sDbl +        reader.GetDecimal(16).ToString().Trim() + sDbl);	// 保険料
					sbData.Append(         sDbl + sSng + reader.GetString(0).Trim()              + sDbl);	// 地区１ＣＤ
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(1).Trim()              + sDbl);	// 地区２ＣＤ
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(2).Trim()              + sDbl);	// 店所ＣＤ
					sbData.Append(sKanma + sDbl +        reader.GetString(3).Trim()              + sDbl);	// 店所名
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(4).Trim()              + sDbl);	// 会員ＣＤ
					sbData.Append(sKanma + sDbl +        reader.GetString(5).Trim()              + sDbl);	// 店所名
					sbData.Append(sKanma + sDbl + sSng + reader.GetString(6).Trim()              + sDbl);	// 部門ＣＤ
					sbData.Append(sKanma + sDbl +        reader.GetString(7).Trim()              + sDbl);	// 部門名

					sbData.Append(sKanma + sDbl +        reader.GetDecimal(8).ToString().Trim() + sDbl);	// 件数
					sbData.Append(sKanma + sDbl +        reader.GetDecimal(9).ToString().Trim() + sDbl);	// 個数
					sbData.Append(sKanma + sDbl +        reader.GetDecimal(10).ToString().Trim() + sDbl);	// 重量
					sbData.Append(sKanma + sDbl +        reader.GetDecimal(11).ToString().Trim() + sDbl);	// 運賃
					sbData.Append(sKanma + sDbl +        reader.GetDecimal(12).ToString().Trim() + sDbl);	// 中継費
					sbData.Append(sKanma + sDbl +        reader.GetDecimal(13).ToString().Trim() + sDbl);	// 保険料
// MOD 2007.02.01 東都）高木 出荷状況一覧表の変更 END

					sList.Add(sbData);
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
					sRet[0] = "該当データがありません";
				else
				{
					sRet[0] = "正常終了";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return sRet;
		}

		/*********************************************************************
		 * 出荷状況一覧表（印刷用）２
		 * 引数：会員情報、地区（開始、終了）、店所（開始、終了）、
		 *		 出荷日（開始、終了）
		 * 戻値：ステータス、店所名、得意先部課名、...
		 *
		 *********************************************************************/
		[WebMethod]
		public ArrayList Get_syuuka_Prn2(string[] sUser, string[] sData)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "出荷状況一覧表出力用取得２開始");

			OracleConnection conn2 = null;
			ArrayList alRet = new ArrayList();

			string[] sRet = new string[1];
			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				alRet.Insert(0, sRet);
				return alRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				alRet.Insert(0, sRet);
//				return alRet;
//			}

			StringBuilder sbQuery  = new StringBuilder(1024);
			try
			{
				sbQuery.Append(GET_SYUUKA_INF2_SELECT);
				sbQuery.Append("   WHERE ST04.出荷日 BETWEEN '"+ sData[4] + "' AND '"+ sData[5] +"' \n");

// ADD 2007.02.01 東都）高木 出荷状況一覧表の変更 START
				//店所
				if(sData[2].Length > 0)
				{
					if(sData[3].Length > 0)
					{
						sbQuery.Append(" AND ST04.店所ＣＤ  BETWEEN '"+ sData[2] + "' AND '"+ sData[3] +"' \n");
					}
					else
					{
						sbQuery.Append(" AND ST04.店所ＣＤ = '"+ sData[2] + "' \n");
					}
				}
				sbQuery.Append(" GROUP BY ST04.店所ＣＤ, ST04.会員ＣＤ, ST04.部門ＣＤ \n");
				sbQuery.Append(GET_SYUUKA_INF2_FROM);
// ADD 2007.02.01 東都）高木 出荷状況一覧表の変更 END

				//地区
				if(sData[0].Length > 0)
				{
// MOD 2007.02.01 東都）高木 出荷状況一覧表の変更 START
//					sbQuery.Append(" AND ST04.店所ＣＤ = CM10.店所ＣＤ \n");
					sbQuery.Append(" WHERE ST04W.店所ＣＤ = CM10.店所ＣＤ \n");
// MOD 2007.02.01 東都）高木 出荷状況一覧表の変更 END
					if(sData[1].Length > 0){
						sbQuery.Append(" AND CM10.地区１  BETWEEN '"+ sData[0] + "' AND '"+ sData[1] +"' \n");
					}
					else
					{
						sbQuery.Append(" AND CM10.地区１ = '"+ sData[0] + "' \n");
					}
				}
				else
				{
// MOD 2007.02.01 東都）高木 出荷状況一覧表の変更 START
//					sbQuery.Append(" AND ST04.店所ＣＤ = CM10.店所ＣＤ(+) \n");
					sbQuery.Append(" WHERE ST04W.店所ＣＤ = CM10.店所ＣＤ(+) \n");
// MOD 2007.02.01 東都）高木 出荷状況一覧表の変更 END
				}

// DEL 2007.02.01 東都）高木 出荷状況一覧表の変更 START
//				//店所
//				if(sData[2].Length > 0)
//				{
//					if(sData[3].Length > 0)
//					{
//						sbQuery.Append(" AND ST04.店所ＣＤ  BETWEEN '"+ sData[2] + "' AND '"+ sData[3] +"' \n");
//					}
//					else
//					{
//						sbQuery.Append(" AND ST04.店所ＣＤ = '"+ sData[2] + "' \n");
//					}
//				}
// DEL 2007.02.01 東都）高木 出荷状況一覧表の変更 END

// MOD 2007.02.01 東都）高木 出荷状況一覧表の変更 START
//				// ＳＭ０１荷送人
//				sbQuery.Append(" AND ST04.会員ＣＤ   = SM01.会員ＣＤ \n");
//				sbQuery.Append(" AND ST04.部門ＣＤ   = SM01.部門ＣＤ \n");
//				sbQuery.Append(" AND ST04.荷送人ＣＤ = SM01.荷送人ＣＤ \n");
//
//				sbQuery.Append(" ORDER BY 出荷日, 地区１, 店所ＣＤ, 得意先ＣＤ, 部課ＣＤ \n");
				// ＣＭ０１会員
				sbQuery.Append(" AND ST04W.会員ＣＤ = CM01.会員ＣＤ(+) \n");
				// ＣＭ０２部門
				sbQuery.Append(" AND ST04W.会員ＣＤ = CM02.会員ＣＤ(+) \n");
				sbQuery.Append(" AND ST04W.部門ＣＤ = CM02.部門ＣＤ(+) \n");

// MOD 2007.02.06 東都）高木 桑田殿テストエラー修正 START
//				sbQuery.Append(" ORDER BY 地区１, 地区２, ST04W.店所ＣＤ, ST04W.会員ＣＤ, ST04W.部門ＣＤ \n");
				sbQuery.Append(" ORDER BY 地区１, 地区２, ST04W.店所ＣＤ, ST04W.部門ＣＤ, ST04W.会員ＣＤ \n");
// MOD 2007.02.06 東都）高木 桑田殿テストエラー修正 END
// MOD 2007.02.01 東都）高木 出荷状況一覧表の変更 END

				OracleDataReader reader = CmdSelect(sUser, conn2, sbQuery);

				while (reader.Read())
				{
// MOD 2007.02.01 東都）高木 出荷状況一覧表の変更 START
//					string[] sbData = new string[12];
//					sbData[0]  = reader.GetString(0).Trim();	// 地区ＣＤ
//					sbData[1]  = reader.GetString(1).Trim();	// 店所ＣＤ
//					sbData[2]  = reader.GetString(2).Trim();	// 店所名
//					sbData[3]  = reader.GetString(4).Trim() + reader.GetString(5).Trim();
//																// 荷主名
//					sbData[4]  = reader.GetString(3).Trim();	// 荷主名ＣＤ
//					sbData[5]  = reader.GetString(9).Trim();	// 出荷日
//					sbData[6]  = reader.GetDecimal(10).ToString().Trim();	// データ件数
//					sbData[7]  = reader.GetDecimal(11).ToString().Trim();	// 個数
//					sbData[8]  = reader.GetDecimal(12).ToString().Trim();	// 重量
//					sbData[9]  = reader.GetDecimal(14).ToString().Trim();	// 運賃
//					sbData[10] = reader.GetDecimal(15).ToString().Trim();	// 中継費
//					sbData[11] = reader.GetDecimal(16).ToString().Trim();	// 保険料
					string[] sbData = new string[14];
					sbData[0]  = reader.GetString(0).Trim();	// 地区１ＣＤ
					sbData[1]  = reader.GetString(1).Trim();	// 地区２ＣＤ
					sbData[2]  = reader.GetString(2).Trim();	// 店所ＣＤ
					sbData[3]  = reader.GetString(3).Trim();	// 店所名
					sbData[4]  = reader.GetString(4).Trim();	// 会員ＣＤ
					sbData[5]  = reader.GetString(5).Trim();	// 会員名
					sbData[6]  = reader.GetString(6).Trim();	// 部門ＣＤ
					sbData[7]  = reader.GetString(7).Trim();	// 部門名

					sbData[8]  = reader.GetDecimal(8).ToString().Trim();	// 件数
					sbData[9]  = reader.GetDecimal(9).ToString().Trim();	// 個数
					sbData[10]  = reader.GetDecimal(10).ToString().Trim();	// 重量
					sbData[11]  = reader.GetDecimal(11).ToString().Trim();	// 運賃
					sbData[12] = reader.GetDecimal(12).ToString().Trim();	// 中継費
					sbData[13] = reader.GetDecimal(13).ToString().Trim();	// 保険料
// MOD 2007.02.01 東都）高木 出荷状況一覧表の変更 END
					alRet.Add(sbData);
				}
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				disposeReader(reader);
				reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END

				if (alRet.Count == 0)
				{
					sRet[0] = "該当データがありません";
					alRet.Add(sRet);
				}
				else
				{
					sRet[0] = "正常終了";
					alRet.Insert(0, sRet);
				}

			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
				alRet.Insert(0, sRet);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
				alRet.Insert(0, sRet);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			return alRet;
		}

// ADD 2006.12.20 東都）高木 出荷状況一覧表の機能追加 END
// ADD 2007.01.12 東都）高木 ご依頼主検索のコピー START
		/*********************************************************************
		 * 依頼主データ取得
		 * 引数：会員ＣＤ、部門ＣＤ、荷送人ＣＤ、店所ＣＤ
		 * 戻値：ステータス、カナ略称、電話番号、郵便番号、住所、名前、重量
		 *		 メールアドレス、得意先ＣＤ、得意先部課ＣＤ、更新日時
		 *********************************************************************/
		private static string GET_SIRAINUSI_SELECT1
			= "SELECT SM01.名前１ \n"
			+ " FROM ＳＭ０１荷送人 SM01 \n"
			+ ", ＣＭ０２部門 CM02 \n"
			+ ", ＣＭ１４郵便番号 CM14 \n"
			+ "";

		private static string GET_SIRAINUSI_SELECT2
			= "SELECT CM02.部門名 \n"
			+ " FROM ＣＭ０２部門 CM02 \n"
			+ ", ＣＭ１４郵便番号 CM14 \n"
			+ "";

		private static string GET_SIRAINUSI_SELECT3
			= "SELECT CM01.会員名 \n"
			+ " FROM ＣＭ０１会員 CM01 \n"
			+ ", ＣＭ０２部門 CM02 \n"
			+ ", ＣＭ１４郵便番号 CM14 \n"
			+ "";

		[WebMethod]
		public String[] Get_Sirainusi(string[] sUser, string sKCode, string sBCode, string sICode, string sTCode)
		{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//			logFileOpen(sUser);
			logWriter(sUser, INF, "依頼主情報取得開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[4]{"","","",""};

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//
//			// 会員チェック
//			sRet[0] = userCheck2(conn2, sUser);
//			if(sRet[0].Length > 0)
//			{
//				disconnect2(sUser, conn2);
//				logFileClose();
//				return sRet;
//			}
//
			try
			{
				string cmdQuery = "";
				OracleDataReader reader;

				if(sKCode.Length > 0){
					cmdQuery = GET_SIRAINUSI_SELECT3
						+ " WHERE CM01.会員ＣＤ = '" + sKCode + "' \n"
						+ " AND CM01.削除ＦＧ = '0' \n"
						+ " AND CM01.会員ＣＤ = CM02.会員ＣＤ \n"
						+ " AND CM02.削除ＦＧ = '0' \n"
						+ " AND CM02.郵便番号 = CM14.郵便番号 \n"
						+ "";

					//店所ＣＤが設定されている時
					if(sTCode.Length > 0){
						cmdQuery += " AND CM14.店所ＣＤ = '" + sTCode + "' \n";
					}

					reader = CmdSelect(sUser, conn2, cmdQuery);

					if(reader.Read()) sRet[1]  = reader.GetString(0).Trim();
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
					disposeReader(reader);
					reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
					if(sBCode.Length > 0){
						cmdQuery = GET_SIRAINUSI_SELECT2
							+ " WHERE CM02.会員ＣＤ = '" + sKCode + "' \n"
							+ " AND CM02.部門ＣＤ = '" + sBCode + "' \n"
							+ " AND CM02.削除ＦＧ = '0' \n"
							+ " AND CM02.郵便番号 = CM14.郵便番号 \n"
							+ "";

						//店所ＣＤが設定されている時
						if(sTCode.Length > 0){
							cmdQuery += " AND CM14.店所ＣＤ = '" + sTCode + "' \n";
						}

						reader = CmdSelect(sUser, conn2, cmdQuery);

						if(reader.Read()) sRet[2]  = reader.GetString(0).Trim();
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
						disposeReader(reader);
						reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END

						if(sICode.Length > 0){
							cmdQuery = GET_SIRAINUSI_SELECT1
								+ " WHERE SM01.会員ＣＤ = '" + sKCode + "' \n"
								+ " AND SM01.部門ＣＤ = '" + sBCode + "' \n"
								+ " AND SM01.荷送人ＣＤ = '" + sICode + "' \n"
								+ " AND SM01.削除ＦＧ = '0' \n"
								+ " AND SM01.会員ＣＤ = CM02.会員ＣＤ \n"
								+ " AND SM01.部門ＣＤ = CM02.部門ＣＤ \n"
								+ " AND CM02.削除ＦＧ = '0' \n"
								+ " AND CM02.郵便番号 = CM14.郵便番号 \n"
								+ "";

							//店所ＣＤが設定されている時
							if(sTCode.Length > 0){
								cmdQuery += " AND CM14.店所ＣＤ = '" + sTCode + "' \n";
							}

							reader = CmdSelect(sUser, conn2, cmdQuery);

							if(reader.Read()) sRet[3]  = reader.GetString(0).Trim();
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
							disposeReader(reader);
							reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
						}
					}else{
						//部門ＣＤが未入力の場合
						if(sICode.Length > 0){
							cmdQuery = GET_SIRAINUSI_SELECT1
								+ " WHERE SM01.会員ＣＤ = '" + sKCode + "' \n"
								+ " AND SM01.荷送人ＣＤ = '" + sICode + "' \n"
								+ " AND SM01.削除ＦＧ = '0' \n"
								+ " AND SM01.会員ＣＤ = CM02.会員ＣＤ \n"
								+ " AND SM01.部門ＣＤ = CM02.部門ＣＤ \n"
								+ " AND CM02.削除ＦＧ = '0' \n"
								+ " AND CM02.郵便番号 = CM14.郵便番号 \n"
								+ "";

							//店所ＣＤが設定されている時
							if(sTCode.Length > 0){
								cmdQuery += " AND CM14.店所ＣＤ = '" + sTCode + "' \n";
							}

							reader = CmdSelect(sUser, conn2, cmdQuery);

							if(reader.Read()) sRet[3]  = reader.GetString(0).Trim();
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
							disposeReader(reader);
							reader = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
						}
					}
				}

				sRet[0] = "正常終了";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 START
				conn2 = null;
// ADD 2007.04.28 東都）高木 オブジェクトの破棄 END
// DEL 2007.05.10 東都）高木 未使用関数のコメント化
//				logFileClose();
			}
			
			return sRet;
		}
// ADD 2007.01.12 東都）高木 ご依頼主検索のコピー END

// ADD 2007.11.14 KCL) 森本 global対応の為、お客様コード確認をコピー START
		/*********************************************************************
		 * 依頼主情報取得２
		 * 引数：ユーザー、会員ＣＤ、部門ＣＤ、荷送人ＣＤ、店所ＣＤ
		 * 戻値：依頼主情報
		 *
		 * 参照元：出荷照会.cs
		 *********************************************************************/
		[WebMethod]
		public String[] Get_Sirainusi2(string[] sUser, string sKCode, string sBCode, string sICode, string sTCode)
		{
			logWriter(sUser, INF, "依頼主情報取得２開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[4]{"","","",""};

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}

			try
			{
				string cmdQuery = "";
				OracleDataReader reader;

				if(sKCode.Length > 0)
				{
					cmdQuery = GET_SIRAINUSI_SELECT3
						+ " WHERE CM01.会員ＣＤ = '" + sKCode + "' \n"
						+ " AND CM01.削除ＦＧ = '0' \n"
						+ " AND CM01.会員ＣＤ = CM02.会員ＣＤ \n"
						+ " AND CM02.削除ＦＧ = '0' \n"
						+ " AND CM02.郵便番号 = CM14.郵便番号 \n"
						+ "";

					//店所ＣＤが設定されている時
					if(sTCode.Length > 0)
					{
//保留 MOD 2010.07.21 東都）高木 リコー様対応 START
						cmdQuery += " AND CM14.店所ＣＤ = '" + sTCode + "' \n";
//						cmdQuery += " AND CM14.店所ＣＤ IN ('" + sTCode + "','030') \n";
//保留 MOD 2010.07.21 東都）高木 リコー様対応 END
					}
// MOD 2010.04.13 東都）高木 郵便番号が削除された時の障害対応 START
//					cmdQuery += " AND CM14.削除ＦＧ = '0' \n";
// MOD 2010.04.13 東都）高木 郵便番号が削除された時の障害対応 END
					//店所ＣＤが設定されている時
					if (sTCode.Length > 0) 
					{
						cmdQuery += "UNION \n";
						cmdQuery += "SELECT CM01.会員名 \n"
							+ " FROM ＣＭ０１会員 CM01 \n"
							+ "     ,ＣＭ０５会員扱店 CM05 \n"
							+ " WHERE CM01.会員ＣＤ = '" + sKCode + "' \n"
							+ " AND CM01.削除ＦＧ = '0' \n"
							+ " AND CM01.会員ＣＤ = CM05.会員ＣＤ \n"
							+ " AND CM05.削除ＦＧ = '0' \n"
							+ " AND CM05.店所ＣＤ = '" + sTCode + "' \n";
// MOD 2016.04.08 bevas) 松本 社内伝票機能追加対応 START
						cmdQuery += "UNION \n";
						cmdQuery += "SELECT CM01.会員名 \n"
							+ " FROM ＣＭ０１会員 CM01 \n"
							+ "     ,ＣＭ０５会員扱店Ｆ CM05F \n"
							+ " WHERE CM01.会員ＣＤ = '" + sKCode + "' \n"
							+ " AND CM01.削除ＦＧ = '0' \n"
							+ " AND CM01.会員ＣＤ = CM05F.会員ＣＤ \n"
							+ " AND CM05F.削除ＦＧ = '0' \n"
							+ " AND CM05F.店所ＣＤ = '" + sTCode + "' \n";
// MOD 2016.04.08 bevas) 松本 社内伝票機能追加対応 END
					}

					reader = CmdSelect(sUser, conn2, cmdQuery);

					if(reader.Read()) sRet[1]  = reader.GetString(0).Trim();
					disposeReader(reader);
					reader = null;

					if(sBCode.Length > 0)
					{
						cmdQuery = GET_SIRAINUSI_SELECT2
							+ " WHERE CM02.会員ＣＤ = '" + sKCode + "' \n"
							+ " AND CM02.部門ＣＤ = '" + sBCode + "' \n"
							+ " AND CM02.削除ＦＧ = '0' \n"
							+ " AND CM02.郵便番号 = CM14.郵便番号 \n"
							+ "";

						//店所ＣＤが設定されている時
						if(sTCode.Length > 0)
						{
//保留 MOD 2010.07.21 東都）高木 リコー様対応 START
							cmdQuery += " AND CM14.店所ＣＤ = '" + sTCode + "' \n";
//							cmdQuery += " AND CM14.店所ＣＤ IN ('" + sTCode + "','030') \n";
//保留 MOD 2010.07.21 東都）高木 リコー様対応 END
						}

// MOD 2010.04.13 東都）高木 郵便番号が削除された時の障害対応 START
//						cmdQuery += " AND CM14.削除ＦＧ = '0' \n";
// MOD 2010.04.13 東都）高木 郵便番号が削除された時の障害対応 END
						//店所ＣＤが設定されている時
						if (sTCode.Length > 0) 
						{
							cmdQuery += "UNION \n";
							cmdQuery += "SELECT CM02.部門名 \n"
								+ " FROM ＣＭ０２部門 CM02 \n"
								+ "     ,ＣＭ０５会員扱店 CM05 \n"
								+ " WHERE CM02.会員ＣＤ = '" + sKCode + "' \n"
								+ " AND CM02.部門ＣＤ = '" + sBCode + "' \n"
								+ " AND CM02.削除ＦＧ = '0' \n"
								+ " AND CM02.会員ＣＤ = CM05.会員ＣＤ \n"
								+ " AND CM05.削除ＦＧ = '0' \n"
								+ " AND CM05.店所ＣＤ = '" + sTCode + "' \n";
// MOD 2016.04.08 bevas) 松本 社内伝票機能追加対応 START
							cmdQuery += "UNION \n";
							cmdQuery += "SELECT CM02.部門名 \n"
								+ " FROM ＣＭ０２部門 CM02 \n"
								+ "     ,ＣＭ０５会員扱店Ｆ CM05F \n"
								+ " WHERE CM02.会員ＣＤ = '" + sKCode + "' \n"
								+ " AND CM02.部門ＣＤ = '" + sBCode + "' \n"
								+ " AND CM02.削除ＦＧ = '0' \n"
								+ " AND CM02.会員ＣＤ = CM05F.会員ＣＤ \n"
								+ " AND CM05F.削除ＦＧ = '0' \n"
								+ " AND CM05F.店所ＣＤ = '" + sTCode + "' \n";
// MOD 2016.04.08 bevas) 松本 社内伝票機能追加対応 END
						}

						reader = CmdSelect(sUser, conn2, cmdQuery);

						if(reader.Read()) sRet[2]  = reader.GetString(0).Trim();
						disposeReader(reader);
						reader = null;

						if(sICode.Length > 0)
						{
							cmdQuery = GET_SIRAINUSI_SELECT1
								+ " WHERE SM01.会員ＣＤ = '" + sKCode + "' \n"
								+ " AND SM01.部門ＣＤ = '" + sBCode + "' \n"
								+ " AND SM01.荷送人ＣＤ = '" + sICode + "' \n"
								+ " AND SM01.削除ＦＧ = '0' \n"
								+ " AND SM01.会員ＣＤ = CM02.会員ＣＤ \n"
								+ " AND SM01.部門ＣＤ = CM02.部門ＣＤ \n"
								+ " AND CM02.削除ＦＧ = '0' \n"
								+ " AND CM02.郵便番号 = CM14.郵便番号 \n"
								+ "";

							//店所ＣＤが設定されている時
							if(sTCode.Length > 0)
							{
//保留 MOD 2010.07.21 東都）高木 リコー様対応 START
								cmdQuery += " AND CM14.店所ＣＤ = '" + sTCode + "' \n";
//								cmdQuery += " AND CM14.店所ＣＤ IN ('" + sTCode + "','030') \n";
//保留 MOD 2010.07.21 東都）高木 リコー様対応 END
							}

// MOD 2010.04.13 東都）高木 郵便番号が削除された時の障害対応 START
//							cmdQuery += " AND CM14.削除ＦＧ = '0' \n";
// MOD 2010.04.13 東都）高木 郵便番号が削除された時の障害対応 END
							//店所ＣＤが設定されている時
							if (sTCode.Length > 0) 
							{
								cmdQuery += "UNION \n";
								cmdQuery += "SELECT SM01.名前１ \n"
									+ " FROM ＳＭ０１荷送人 SM01 \n"
									+ "     ,ＣＭ０５会員扱店 CM05 \n"
									+ " WHERE SM01.会員ＣＤ = '" + sKCode + "' \n"
									+ " AND SM01.部門ＣＤ = '" + sBCode + "' \n"
									+ " AND SM01.荷送人ＣＤ = '" + sICode + "' \n"
									+ " AND SM01.削除ＦＧ = '0' \n"
									+ " AND SM01.会員ＣＤ = CM05.会員ＣＤ \n"
									+ " AND CM05.削除ＦＧ = '0' \n"
									+ " AND CM05.店所ＣＤ = '" + sTCode + "' \n";
// MOD 2016.04.08 bevas) 松本 社内伝票機能追加対応 START
								cmdQuery += "UNION \n";
								cmdQuery += "SELECT SM01.名前１ \n"
									+ " FROM ＳＭ０１荷送人 SM01 \n"
									+ "     ,ＣＭ０５会員扱店Ｆ CM05F \n"
									+ " WHERE SM01.会員ＣＤ = '" + sKCode + "' \n"
									+ " AND SM01.部門ＣＤ = '" + sBCode + "' \n"
									+ " AND SM01.荷送人ＣＤ = '" + sICode + "' \n"
									+ " AND SM01.削除ＦＧ = '0' \n"
									+ " AND SM01.会員ＣＤ = CM05F.会員ＣＤ \n"
									+ " AND CM05F.削除ＦＧ = '0' \n"
									+ " AND CM05F.店所ＣＤ = '" + sTCode + "' \n";
// MOD 2016.04.08 bevas) 松本 社内伝票機能追加対応 END
							}

							reader = CmdSelect(sUser, conn2, cmdQuery);

							if(reader.Read()) sRet[3]  = reader.GetString(0).Trim();
							disposeReader(reader);
							reader = null;
						}
					}
					else
					{
						//部門ＣＤが未入力の場合
						if(sICode.Length > 0)
						{
							cmdQuery = GET_SIRAINUSI_SELECT1
								+ " WHERE SM01.会員ＣＤ = '" + sKCode + "' \n"
								+ " AND SM01.荷送人ＣＤ = '" + sICode + "' \n"
								+ " AND SM01.削除ＦＧ = '0' \n"
								+ " AND SM01.会員ＣＤ = CM02.会員ＣＤ \n"
								+ " AND SM01.部門ＣＤ = CM02.部門ＣＤ \n"
								+ " AND CM02.削除ＦＧ = '0' \n"
								+ " AND CM02.郵便番号 = CM14.郵便番号 \n"
								+ "";

							//店所ＣＤが設定されている時
							if(sTCode.Length > 0)
							{
//保留 MOD 2010.07.21 東都）高木 リコー様対応 START
								cmdQuery += " AND CM14.店所ＣＤ = '" + sTCode + "' \n";
//							cmdQuery += " AND CM14.店所ＣＤ IN ('" + sTCode + "','030') \n";
//保留 MOD 2010.07.21 東都）高木 リコー様対応 END
							}

// MOD 2010.04.13 東都）高木 郵便番号が削除された時の障害対応 START
//							cmdQuery += " AND CM14.削除ＦＧ = '0' \n";
// MOD 2010.04.13 東都）高木 郵便番号が削除された時の障害対応 END
							//店所ＣＤが設定されている時
							if (sTCode.Length > 0) 
							{
								cmdQuery += "UNION \n";
								cmdQuery += "SELECT SM01.名前１ \n"
									+ " FROM ＳＭ０１荷送人 SM01 \n"
									+ "     ,ＣＭ０５会員扱店 CM05 \n"
									+ " WHERE SM01.会員ＣＤ = '" + sKCode + "' \n"
									+ " AND SM01.荷送人ＣＤ = '" + sICode + "' \n"
									+ " AND SM01.削除ＦＧ = '0' \n"
									+ " AND SM01.会員ＣＤ = CM05.会員ＣＤ \n"
									+ " AND CM05.削除ＦＧ = '0' \n"
									+ " AND CM05.店所ＣＤ = '" + sTCode + "' \n";
// MOD 2016.04.08 bevas) 松本 社内伝票機能追加対応 START
								cmdQuery += "UNION \n";
								cmdQuery += "SELECT SM01.名前１ \n"
									+ " FROM ＳＭ０１荷送人 SM01 \n"
									+ "     ,ＣＭ０５会員扱店Ｆ CM05F \n"
									+ " WHERE SM01.会員ＣＤ = '" + sKCode + "' \n"
									+ " AND SM01.荷送人ＣＤ = '" + sICode + "' \n"
									+ " AND SM01.削除ＦＧ = '0' \n"
									+ " AND SM01.会員ＣＤ = CM05F.会員ＣＤ \n"
									+ " AND CM05F.削除ＦＧ = '0' \n"
									+ " AND CM05F.店所ＣＤ = '" + sTCode + "' \n";
// MOD 2016.04.08 bevas) 松本 社内伝票機能追加対応 END
							}

							reader = CmdSelect(sUser, conn2, cmdQuery);

							if(reader.Read()) sRet[3]  = reader.GetString(0).Trim();
							disposeReader(reader);
							reader = null;
						}
					}
				}

				sRet[0] = "正常終了";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			
			return sRet;
		}
// ADD 2007.11.14 KCL) 森本 global対応の為、お客様コード確認をコピー END
// ADD 2008.02.14 東都）高木 セッション数の取得 START
		/*********************************************************************
		 * ＤＢセッション数の取得
		 * 引数：ユーザー
		 * 戻値：ＤＢセッション情報
		 *
		 *********************************************************************/
		private static string GET_DBSESSIONCOUNT_CONNECT
			= "User Id=SYSTEM;Password=MANAGER;Data Source="
			+ "IS2DB01;Pooling=false;Connection Timeout=180"
			;
		private static string GET_DBSESSIONCOUNT_SELECT
			= "SELECT TERMINAL, USERNAME, PROGRAM, STATUS, COUNT(*) \n"
			+ "FROM V$SESSION \n"
			+ "GROUP BY TERMINAL, USERNAME, PROGRAM, STATUS \n"
			+ "ORDER BY TERMINAL, USERNAME, PROGRAM, STATUS \n"
			;
		private static int GET_DBSESSIONCOUNT_ROWS = 20;
		[WebMethod]
		public string[][] Get_DBSessionCount(string[] sUser)
		{
			logWriter(sUser, INF, "セッション数の取得");

			string[][] sRet;
			sRet = new string[GET_DBSESSIONCOUNT_ROWS][];
			sRet[0] = new string[]{""};

			OracleConnection conn   = null;
			OracleDataReader reader = null;

			try
			{
				// ＤＢ接続
				conn = new OracleConnection(GET_DBSESSIONCOUNT_CONNECT);
				conn.Open();

				OracleCommand cmd = new OracleCommand(GET_DBSESSIONCOUNT_SELECT);
				cmd.Connection  = conn;
				cmd.CommandType = CommandType.Text;

				cmd.Prepare();
				reader = cmd.ExecuteReader();
				cmd.Dispose();

				sRet = new string[GET_DBSESSIONCOUNT_ROWS][];
				sRet[0] = new string[]{""};
				int iRow = 1;
				while(reader.Read()){
					sRet[iRow] = new string[reader.FieldCount];
					for(int iCol = 0; iCol < reader.FieldCount; iCol++)
					{
						sRet[iRow][iCol] = reader.GetValue(iCol).ToString();
					}
					iRow++;
					if(iRow >= GET_DBSESSIONCOUNT_ROWS) break;
				}
			}
			catch (OracleException ex)
			{
				sRet[0][0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0][0] = ex.Message;
				logWriter(sUser, ERR, "サーバエラー：" + ex.Message);
			}
			finally
			{
				if(reader != null){
					try{ reader.Close(); } 
					catch (Exception){};
					try{ reader.Dispose(); } 
					catch (Exception){};
					reader = null;
				}

				try{
					conn.Close();
				}catch (Exception ex){
					logWriter(sUser, ERR, "切断エラー：" + ex.Message);
				}
				try{
					conn.Dispose();
				}catch (Exception ex){
					logWriter(sUser, ERR, "破棄エラー：" + ex.Message);
				}
				conn = null;
			}

			return sRet;
		}
// ADD 2008.02.14 東都）高木 セッション数の取得 END
// ADD 2009.01.06 東都）高木 パスワードチェック対応 START
		/*********************************************************************
		 * パスワード更新日リスト取得
		 * 引数：ユーザー情報、パスワード更新日開始、終了
		 * 戻値：パスワード更新日リスト
		 *
		 *********************************************************************/
		private static string GET_PASSUPDDATE_SELECT1
			= "SELECT CM04.登録ＰＧ, COUNT(*) \n"
			+ " FROM ＣＭ０１会員 CM01 \n"
			+ " , ＣＭ０２部門 CM02 \n"
			+ " , ＣＭ０４利用者 CM04 \n"
			+ " WHERE CM01.使用終了日 >= TO_CHAR(SYSDATE,'YYYYMMDD') \n"
			+ " AND CM01.会員ＣＤ = CM02.会員ＣＤ \n"
			+ " AND CM02.会員ＣＤ = CM04.会員ＣＤ \n"
			+ " AND CM02.部門ＣＤ = CM04.部門ＣＤ \n"
			+ " AND CM01.削除ＦＧ = '0' \n"
			+ " AND CM02.削除ＦＧ = '0' \n"
			;

		private static string GET_PASSUPDDATE_WHERE1
			= " AND CM04.削除ＦＧ = '0' \n"
			+ " GROUP BY CM04.登録ＰＧ \n"
			+ " ORDER BY CM04.登録ＰＧ \n"
			;

		[WebMethod]
		public String[] Get_PassUpdDate(string[] sUser, string sDateS, string sDateE)
		{
			logWriter(sUser, INF, "パスワード更新日リスト取得開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[32];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}

			StringBuilder sbQuery = new StringBuilder(1024);
			StringBuilder sbRet = new StringBuilder(1024);
			try
			{
				OracleDataReader reader;

				sbQuery.Append(GET_PASSUPDDATE_SELECT1);
				if(sDateE == null || sDateE.Length == 0){
					sbQuery.Append(" AND CM04.登録ＰＧ = '"+sDateS+"' \n");
				}else if(sDateS == null || sDateS.Length == 0){
					sbQuery.Append(" AND CM04.登録ＰＧ <= '"+sDateE+"' \n");
				}else{
					sbQuery.Append(" AND CM04.登録ＰＧ BETWEEN '"+sDateS+"' AND '"+sDateE+"' \n");
				}
				sbQuery.Append(GET_PASSUPDDATE_WHERE1);

				reader = CmdSelect(sUser, conn2, sbQuery);

				int iCnt = 1;
				while(reader.Read() && iCnt < sRet.Length){
					sbRet = new StringBuilder(1024);
					sbRet.Append(sSepa + reader.GetString(0).Trim());
					sbRet.Append(sSepa + reader.GetString(1).Trim());
					sbRet.Append(sSepa);
					sRet[iCnt] = sbRet.ToString();
					iCnt++;
				}
				disposeReader(reader);
				reader = null;

				sRet[0] = "正常終了";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				sbQuery = null;
				sbRet = null;
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			
			return sRet;
		}

		/*********************************************************************
		 * パスワード更新日一括更新
		 * 引数：ユーザー情報、パスワード更新日旧、パスワード更新日新、件数
		 * 戻値：結果
		 *
		 *********************************************************************/
		private static string UPD_PASSUPDDATE_WHERE1
			= " WHERE (会員ＣＤ, 利用者ＣＤ) \n"
			+ " IN ( \n"
			+ " SELECT CM04.会員ＣＤ, CM04.利用者ＣＤ \n"
			+ " FROM ＣＭ０１会員 CM01 \n"
			+ " , ＣＭ０２部門 CM02 \n"
			+ " , ＣＭ０４利用者 CM04 \n"
			+ " WHERE CM01.使用終了日 >= TO_CHAR(SYSDATE,'YYYYMMDD') \n"
			+ " AND CM01.会員ＣＤ = CM02.会員ＣＤ \n"
			+ " AND CM02.会員ＣＤ = CM04.会員ＣＤ \n"
			+ " AND CM02.部門ＣＤ = CM04.部門ＣＤ \n"
			+ " AND CM01.削除ＦＧ = '0' \n"
			+ " AND CM02.削除ＦＧ = '0' \n"
			;
		private static string UPD_PASSUPDDATE_WHERE2
			= " AND CM04.削除ＦＧ = '0' \n"
			+ " ) \n"
			;

		[WebMethod]
		public String[] Upd_PassUpdDate(string[] sUser, string sDateOld, string sDateNew, int iNum)
		{
			logWriter(sUser, INF, "パスワード更新日一括更新開始");
			string[] sRet = new string[1]{""};
			OracleConnection conn2 = null;

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}

			OracleTransaction tran;
			tran = conn2.BeginTransaction();
			StringBuilder sbQuery = new StringBuilder(1024);
			try
			{
				sbQuery.Append("UPDATE ＣＭ０４利用者 \n");
				sbQuery.Append(" SET 登録ＰＧ = '"+sDateNew+"' \n");
				sbQuery.Append(" , 更新ＰＧ = 'パス一括' \n");
				sbQuery.Append(" , 更新者 = '"+sUser[1]+"' \n");

				sbQuery.Append(UPD_PASSUPDDATE_WHERE1);
				sbQuery.Append(" AND CM04.登録ＰＧ = '"+sDateOld+"' \n");
				sbQuery.Append(UPD_PASSUPDDATE_WHERE2);
				if(iNum > 0){
					sbQuery.Append(" AND ROWNUM <= "+iNum+" \n");
				}

				CmdUpdate(sUser, conn2, sbQuery);

				sRet[0] = "正常終了";
				tran.Commit();
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				sbQuery = null;
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			
			return sRet;
		}
// ADD 2009.01.06 東都）高木 パスワードチェック対応 END
//保留　お客様リストの作成
// ADD 2009.04.02 東都）高木 稼働日対応 START
		/*********************************************************************
		 * 稼働日取得
		 * 引数：ユーザー情報、開始日、終了日
		 * 戻値：結果
		 *
		 *********************************************************************/
		private static string GET_KADOBI_SELECT
			= " SELECT CM07.年月日, CM07.稼働日ＦＧ, CM07.その他ＦＧ \n"
			+ " FROM ＣＭ０７稼働日 CM07 \n"
			;

		[WebMethod]
		public object[] Get_Kadobi(string[] sUser, string sDateStart, string sDateEnd)
		{
			logWriter(sUser, INF, "稼働日取得開始");
			string   sRet  = "";
			string[] sKadouFG = new string[32];
			string[] sOtherFG = new string[32];
			OracleConnection conn2 = null;

			int iCnt;
			for(iCnt = 0; iCnt < sKadouFG.Length; iCnt++){
				sKadouFG[iCnt] = "";
				sOtherFG[iCnt] = "";
			}

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet = "ＤＢ接続エラー";
				return new object[]{sRet};
			}

			StringBuilder sbQuery = new StringBuilder(1024);
			try
			{
				OracleDataReader reader;

				sbQuery.Append(GET_KADOBI_SELECT);
				sbQuery.Append(" WHERE CM07.年月日 >= " + sDateStart + " \n");
				sbQuery.Append(  " AND CM07.年月日 <= " + sDateEnd + " \n");
				sbQuery.Append(  " AND CM07.削除ＦＧ = '0' \n");
				sbQuery.Append(" ORDER BY CM07.年月日 \n");

				reader = CmdSelect(sUser, conn2, sbQuery);

				iCnt = 1;
				while(reader.Read() && iCnt < sKadouFG.Length){
					sKadouFG[iCnt]  = reader.GetString(1);
					sOtherFG[iCnt]  = reader.GetString(2);
					iCnt++;
				}

				disposeReader(reader);
				reader = null;
				sRet = "正常終了";
				logWriter(sUser, INF, sRet);
			}
			catch (OracleException ex)
			{
				sRet = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet);
			}
			finally
			{
				sbQuery = null;
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			
			return new object[]{sRet, sKadouFG, sOtherFG};
		}

		/*********************************************************************
		 * 稼働日更新
		 * 引数：ユーザー情報、開始日、データ（※１ヶ月分を超えないこと）
		 * 戻値：結果
		 *
		 *********************************************************************/
		[WebMethod]
		public String[] Upd_Kadobi(string[] sUser, string sDateStart, char[] cKadouFG, char[] cOtherFG)
		{
			logWriter(sUser, INF, "稼働日更新開始");
			string[] sRet = new string[1]{""};
			OracleConnection conn2 = null;
			string s更新日時 = System.DateTime.Now.ToString("yyyyMMddHHmmss");

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}

			OracleTransaction tran;
			tran = conn2.BeginTransaction();
			StringBuilder sbQuery;
			try
			{
				for(int iCnt = 0; iCnt < cKadouFG.Length; iCnt++){
					sbQuery = new StringBuilder(1024);
					sbQuery.Append("UPDATE ＣＭ０７稼働日 \n");
					sbQuery.Append(" SET 稼働日ＦＧ = '"+ cKadouFG[iCnt] +"' \n");
					sbQuery.Append(" ,その他ＦＧ = '"+ cOtherFG[iCnt] +"' \n");
					sbQuery.Append(" ,削除ＦＧ = '0' \n");
					sbQuery.Append(" ,更新日時 = "+ s更新日時 +" \n");
					sbQuery.Append(" ,更新ＰＧ = '稼動更新' \n");
					sbQuery.Append(" ,更新者 = '"+ sUser[1] +"' \n");
					sbQuery.Append(" WHERE 年月日 = "+ sDateStart +" + "+ iCnt +" \n");

					int iUpdCnt = CmdUpdate(sUser, conn2, sbQuery);
					if(iUpdCnt == 0){
						sbQuery = new StringBuilder(1024);
						sbQuery.Append("INSERT INTO ＣＭ０７稼働日 VALUES( \n");
						sbQuery.Append(" "+ sDateStart +" + "+ iCnt +" \n");
						sbQuery.Append(" ,'"+ cKadouFG[iCnt] +"' \n");
						sbQuery.Append(" ,'"+ cOtherFG[iCnt] +"' \n");
						sbQuery.Append(" ,'0' \n");
						sbQuery.Append(" ,"+ s更新日時 +" \n");
						sbQuery.Append(" ,'稼動更新' \n");
						sbQuery.Append(" ,'"+ sUser[1] +"' \n");
						sbQuery.Append(" ,"+ s更新日時 +" \n");
						sbQuery.Append(" ,'稼動更新' \n");
						sbQuery.Append(" ,'"+ sUser[1] +"' \n");
						sbQuery.Append(" ) \n");
						CmdUpdate(sUser, conn2, sbQuery);
					}
				}

				sRet[0] = "正常終了";
				tran.Commit();
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				sbQuery = null;
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			
			return sRet;
		}
// ADD 2009.04.02 東都）高木 稼働日対応 END
// MOD 2009.05.28 東都）高木 出荷実績一覧運賃非表示対応 START
		/*********************************************************************
		 * 会員マスタ更新２
		 * 引数：会員ＣＤ、会員名...
		 * 戻値：ステータス、更新日時
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Upd_Member2(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "会員マスタ更新２開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[2];
			string s更新日時 = System.DateTime.Now.ToString("yyyyMMddHHmmss");

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "UPDATE ＣＭ０１会員 \n"
					+   " SET 記事連携ＦＧ = '" + sKey[1] + "' "
					+       ",更新日時 = " + s更新日時
					+       ",更新ＰＧ = '" + sKey[3] + "' "
					+       ",更新者 = '" + sKey[4] + "' \n"
					+ " WHERE 会員ＣＤ = '" + sKey[0] + "' \n"
					+   " AND 削除ＦＧ = '0' \n"
					+   " AND 更新日時 = " + sKey[2] + " \n";

				if (CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
					tran.Commit();
					sRet[0] = "正常終了";
					sRet[1] = s更新日時;
				}
				else
				{
					tran.Rollback();
					sRet[0] = "他の端末で更新されています";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}
// MOD 2009.05.28 東都）高木 出荷実績一覧運賃非表示対応 END
// MOD 2009.07.09 東都）高木 配完情報検索機能の追加 START
		/*********************************************************************
		 * 配完情報検索
		 * 引数：会員ＣＤ、会員名...
		 * 戻値：ステータス、更新日時
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_Haikan(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "配完情報検索開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[30];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}

			string cmdQuery = "";
			try
			{
				OracleDataReader reader;

				cmdQuery
					= "SELECT * FROM ＧＴ０２配完 \n"
					+ " WHERE 原票番号 = '" + sKey[0] + "' \n"
					;

				reader = CmdSelect(sUser, conn2, cmdQuery);
				if(reader.Read())
				{									 // 原票番号
					sRet[01] = reader.GetString(01); // 集荷店ＣＤ
					sRet[02] = reader.GetString(02); // 集荷日
					sRet[03] = reader.GetString(03); // 集荷時刻
					sRet[04] = reader.GetString(04); // 発送店ＣＤ
					sRet[05] = reader.GetString(05); // 発送日
					sRet[06] = reader.GetString(06); // 到着店ＣＤ
					sRet[07] = reader.GetString(07); // 到着日
					sRet[08] = reader.GetString(08); // 到着時刻
					sRet[09] = reader.GetString(09); // 持出店ＣＤ
					sRet[10] = reader.GetString(10); // 持出日
// MOD 2009.07.09 東都）高木 配完情報検索機能の追加 START
					sRet[11] = reader.GetString(11); // 持出時刻
					sRet[12] = reader.GetString(12); // 配完店ＣＤ
					sRet[13] = reader.GetString(13); // 配完日
					sRet[14] = reader.GetString(14); // 配完時刻
					sRet[15] = reader.GetString(15); // 理由ＣＤ
					sRet[16] = reader.GetString(16); // 削除ＦＧ
					sRet[17] = reader.GetDecimal(17).ToString(); // 登録日時
					sRet[18] = reader.GetString(18); // 登録ＰＧ
					sRet[19] = reader.GetString(19); // 登録者
					sRet[20] = reader.GetDecimal(20).ToString(); // 更新日時
					sRet[21] = reader.GetString(21); // 更新ＰＧ
					sRet[22] = reader.GetString(22); // 更新者
// MOD 2009.07.09 東都）高木 配完情報検索機能の追加 END
				}
				disposeReader(reader);
				reader = null;
				sRet[0] = "正常終了";
				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}
// MOD 2009.07.09 東都）高木 配完情報検索機能の追加 END
// MOD 2010.04.30 東都）高木 ＣＳＶ出力機能の追加 START
		/*********************************************************************
		 * 出荷一覧取得（ＣＳＶ出力用）
		 * 引数：会員ＣＤ、部門ＣＤ、荷受人ＣＤ、荷送人ＣＤ、出荷日 or 登録日、
		 *		 開始日、終了日、状態
		 * 戻値：ステータス、登録日、ジャーナルＮＯ、荷受人ＣＤ...
		 *
		 *********************************************************************/
		private static string GET_CSVWRITE3_SELECT
			= "SELECT S.登録日, S.出荷日, 送り状番号, S.荷受人ＣＤ, S.郵便番号, \n"
			+ " '(' || TRIM(S.電話番号１) || ')' || TRIM(S.電話番号２) || '-' || S.電話番号３, \n"
			+ " S.住所１, S.住所２, S.住所３, S.名前１, S.名前２, S.特殊計, S.着店ＣＤ, S.着店名, \n"
			+ " S.荷送人ＣＤ, NVL(SM01.郵便番号, ' '), \n"
			+ " NVL(SM01.電話番号１,' '), NVL(SM01.電話番号２,' '), NVL(SM01.電話番号３,' '), \n"
			+ " NVL(SM01.住所１,' '), NVL(SM01.住所２,' '), NVL(SM01.名前１,' '), NVL(SM01.名前２,' '), \n"
			+ " TO_CHAR(S.個数), TO_CHAR(S.重量), \n"
			+ " S.指定日, S.輸送指示１, S.輸送指示２, S.品名記事１, S.品名記事２, S.品名記事３, \n"
			+ " S.元着区分, TO_CHAR(S.保険金額), S.お客様出荷番号, \n"
			+ " S.得意先ＣＤ, S.部課ＣＤ, S.才数, \n"
			+ " S.会員ＣＤ, S.部門ＣＤ, S.\"ジャーナルＮＯ\" \n"
// MOD 2011.04.13 東都）高木 重量入力不可対応 START
			+ ", S.運賃才数, S.運賃重量 \n"
// MOD 2011.04.13 東都）高木 重量入力不可対応 END
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 START
			+ ", NVL(CM01.保留印刷ＦＧ,'0') \n"
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 END
// MOD 2014.03.20 BEVAS）高杉 ＣＳＶ出力に配完日付・時刻を追加 START
			+ ", DECODE(S.処理０３,'          ',' ',('20' || SUBSTR(S.処理０３,1,2) || '/' || SUBSTR(S.処理０３,3,2) || '/' || SUBSTR(S.処理０３,5,2) || ' ' || SUBSTR(S.処理０３,7,2) || ':' || SUBSTR(S.処理０３,9,2))) \n"
// MOD 2014.03.20 BEVAS）高杉 ＣＳＶ出力に配完日付・時刻を追加 END
			;
		private static string GET_CSVWRITE3_FROM_1
//			= " FROM \"ＳＴ０１出荷ジャーナル\" S , ＳＭ０１荷送人 SM01 \n"
			= " FROM \"ＳＴ０１出荷ジャーナル\" ST01 , ＳＭ０１荷送人 SM01 \n"
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 START
			+ ", ＣＭ０１会員 CM01 \n"
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 END
			;
		private static string GET_CSVWRITE3_FROM_2 
//			= " FROM \"ＳＴ０２出荷履歴\"       S , ＳＭ０１荷送人 SM01 \n"
			= " FROM \"ＳＴ０２出荷履歴\"       ST02 , ＳＭ０１荷送人 SM01 \n"
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 START
			+ ", ＣＭ０１会員 CM01 \n"
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 END
			;
		private static string GET_CSVWRITE3_SORT_1
			= " ORDER BY S.出荷日, S.会員ＣＤ, S.部門ＣＤ, S.登録日, S.\"ジャーナルＮＯ\" "
			;
		private static string GET_CSVWRITE3_SORT_2
			= " ORDER BY S.会員ＣＤ, S.部門ＣＤ, S.登録日, S.\"ジャーナルＮＯ\" "
			;

		[WebMethod]
		public String[] Get_csvwrite3(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "ＣＳＶ出力用取得３開始");
			string s会員ＣＤ   = sKey[0];
			string s部門ＣＤ   = sKey[1];
			string s荷送人ＣＤ = sKey[2];
			string s日付区分   = sKey[3];
			string s開始日     = sKey[4];
			string s終了日     = sKey[5];
			string s状態ＣＤ   = sKey[6];
			bool   b未出荷     = sKey[6].Equals("90");
			string s送り状番号 = sKey[7];
			string s削除ＦＧ   = sKey[8];
			string s発店ＣＤ   = sKey[9];
			string sＣＳＶ出力形式 = ""; if(sKey.Length > 10) sＣＳＶ出力形式 = sKey[10];
// MOD 2014.03.20 BEVAS）高杉 ＣＳＶ出力に配完日付・時刻を追加 START
			string s配完Ｓ出力形式 = ""; if(sKey.Length > 11) s配完Ｓ出力形式 = sKey[11];
// MOD 2014.03.20 BEVAS）高杉 ＣＳＶ出力に配完日付・時刻を追加 END
//保留 MOD 2010.10.12 東都）高木 運賃エラー対応 START
//保留			bool   b運賃エラー = sKey[6].Equals("91");
//保留			if(b運賃エラー) s削除ＦＧ = "1";
//保留 MOD 2010.10.12 東都）高木 運賃エラー対応 END

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();

			string[] sRet = new string[1];
			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null){
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}

			decimal d才数 = 0;
// MOD 2011.04.13 東都）高木 重量入力不可対応 START
			string  s運賃才数 = "";
			string  s運賃重量 = "";
			decimal d重量 = 0;
			decimal d才数重量 = 0;
// MOD 2011.04.13 東都）高木 重量入力不可対応 END
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 START
			string  s重量入力制御 = "0";
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 END
			StringBuilder sbQuery = new StringBuilder(1024);
			StringBuilder sbQuery2 = new StringBuilder(1024);
			try
			{
				if(s送り状番号.Length == 0){
					if(b未出荷){
						sbQuery.Append(", ＳＴ０５未出荷分 ST05 \n");
						if(s会員ＣＤ.Length > 0){
							sbQuery.Append(" WHERE ST05.会員ＣＤ = '" + s会員ＣＤ + "' \n");
						}else{
							sbQuery.Append(" WHERE ST05.会員ＣＤ > ' ' \n");
						}
						if(s部門ＣＤ.Length > 0){
							sbQuery.Append(" AND ST05.部門ＣＤ = '" + s部門ＣＤ + "' \n");
						}
						if(s日付区分 == "0"){
							sbQuery.Append(" AND ST05.出荷日  BETWEEN '"+ s開始日 + "' AND '"+ s終了日 +"' \n");
						}else{
							sbQuery.Append(" AND ST05.登録日  BETWEEN '"+ s開始日 + "' AND '"+ s終了日 +"' \n");
						}
						sbQuery.Append(" AND ST05.出荷日 < TO_CHAR(SYSDATE,'YYYYMMDD') \n");
						sbQuery.Append(" AND ST05.状態   = '02' \n");
						if(sKey[9].Length > 0){
							sbQuery.Append(" AND ST05.発店ＣＤ = '"+ s発店ＣＤ + "' \n");
						}
						sbQuery.Append(" AND ST05.会員ＣＤ = S.会員ＣＤ \n");
						sbQuery.Append(" AND ST05.部門ＣＤ = S.部門ＣＤ \n");
						sbQuery.Append(" AND ST05.登録日 = S.登録日 \n");
						sbQuery.Append(" AND ST05.\"ジャーナルＮＯ\" = S.\"ジャーナルＮＯ\" \n");
					}else{
						if(s会員ＣＤ.Length > 0){
							sbQuery.Append(" WHERE S.会員ＣＤ = '" + s会員ＣＤ + "' \n");
						}else{
							sbQuery.Append(" WHERE S.会員ＣＤ > ' ' \n");
						}
						if(s部門ＣＤ.Length > 0){
							sbQuery.Append(" AND S.部門ＣＤ = '" + s部門ＣＤ + "' \n");
						}
					}

					if(s荷送人ＣＤ.Length > 0){
						sbQuery.Append(" AND S.荷送人ＣＤ = '"+ s荷送人ＣＤ + "' \n");
					}
					if(s日付区分 == "0"){
						sbQuery.Append(" AND S.出荷日  BETWEEN '"+ s開始日 + "' AND '"+ s終了日 +"' \n");
					}else{
						sbQuery.Append(" AND S.登録日  BETWEEN '"+ s開始日 + "' AND '"+ s終了日 +"' \n");
					}
					
					if(s状態ＣＤ != "00"){
						if(s状態ＣＤ == "aa"){
							sbQuery.Append(" AND S.状態 <> '01' \n");
						}else if(b未出荷){
							sbQuery.Append(" AND S.出荷日 < TO_CHAR(SYSDATE,'YYYYMMDD') \n");
							sbQuery.Append(" AND S.状態   = '02' \n");
//保留 MOD 2010.10.12 東都）高木 運賃エラー対応 START
//保留						}else if(b運賃エラー){
//保留							sbQuery.Append(" AND ( S.運賃 > 0 OR S.中継 > 0 OR S.諸料金 > 0 ) \n");
//保留 MOD 2010.10.12 東都）高木 運賃エラー対応 END
						}else{
							sbQuery.Append(" AND S.状態 = '"+ s状態ＣＤ + "' \n");
						}
					}
				}else{
					sbQuery.Append(" WHERE S.送り状番号 = '"+ s送り状番号 + "' \n");
				}
				if(s削除ＦＧ != "0"){
					if(s削除ＦＧ == "1"){
						sbQuery.Append(" AND S.削除ＦＧ = '1' \n");
					}else{
						sbQuery.Append(" AND S.削除ＦＧ = '0' \n");
					}
				}
				if(s発店ＣＤ.Length > 0){
					sbQuery.Append(" AND S.発店ＣＤ = '"+ s発店ＣＤ + "' \n");
				}
				sbQuery.Append(" AND S.会員ＣＤ   = SM01.会員ＣＤ(+) \n");
				sbQuery.Append(" AND S.部門ＣＤ   = SM01.部門ＣＤ(+) \n");
				sbQuery.Append(" AND S.荷送人ＣＤ = SM01.荷送人ＣＤ(+) \n");
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 START
				sbQuery.Append(" AND S.会員ＣＤ   = CM01.会員ＣＤ(+) \n");
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 END

				OracleDataReader reader;

//				sbQuery2.Append(GET_CSVWRITE3_SELECT);
//				sbQuery2.Append(GET_CSVWRITE3_FROM_1);
//				sbQuery2.Append(sbQuery);
				sbQuery2.Append("SELECT * FROM ( \n");
				sbQuery2.Append(GET_CSVWRITE3_SELECT).Replace("S.","ST01.");
				sbQuery2.Append(GET_CSVWRITE3_FROM_1);
				sbQuery2.Append(sbQuery).Replace("S.","ST01.");
				sbQuery2.Append(" UNION \n");
				sbQuery2.Append(GET_CSVWRITE3_SELECT).Replace("S.","ST02.");
				sbQuery2.Append(GET_CSVWRITE3_FROM_2);
				sbQuery2.Append(sbQuery).Replace("S.","ST02.");
				sbQuery2.Append(") S \n");

				if(s日付区分 == "0"){
					sbQuery2.Append(GET_CSVWRITE3_SORT_1);
				}else{
					sbQuery2.Append(GET_CSVWRITE3_SORT_2);
				}
				reader = CmdSelect(sUser, conn2, sbQuery2);

				StringBuilder sbData = new StringBuilder(1024);
				while (reader.Read()){
					sbData = new StringBuilder(1024);
					if(sＣＳＶ出力形式.Equals("1")){
						sbData.Append(sDbl + sSng + reader.GetString(3).Trim() + sDbl);		// 荷受人ＣＤ
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(5).Trim() + sDbl);	// 荷受人電話番号
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(6).Trim() + sDbl);	// 荷受人住所１
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(7).Trim() + sDbl);	// 荷受人住所２
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(8).Trim() + sDbl);	// 荷受人住所３
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(9).Trim() + sDbl);	// 荷受人名前１
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(10).Trim() + sDbl);	// 荷受人名前２
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(4).Trim() + sDbl);	// 荷受人郵便番号
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(11).Trim() + sDbl);	// 特殊計
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(12).Trim() + sDbl);	// 着店ＣＤ
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(14).Trim() + sDbl);	// 荷送人ＣＤ
						sbData.Append(sKanma + reader.GetString(23).Trim()                     );	// 個数
// MOD 2011.04.13 東都）高木 重量入力不可対応 START
//						sbData.Append(sKanma + reader.GetDecimal(36).ToString().Trim()         );	// 才数
//						sbData.Append(sKanma + reader.GetString(24).Trim()                     );	// 重量
						s運賃才数 = reader.GetString(40).TrimEnd();
						s運賃重量 = reader.GetString(41).TrimEnd();
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 START
						s重量入力制御 = reader.GetString(42).TrimEnd();
						if(s重量入力制御 == "1"
						&& s運賃才数.Length == 0 && s運賃重量.Length == 0
//						&& (reader.GetString(24).TrimEnd() != "0" || reader.GetDecimal(36) != 0)
						){
							sbData.Append(sKanma + reader.GetDecimal(36).ToString().TrimEnd());	// 才数
							sbData.Append(sKanma + reader.GetString(24).TrimEnd());				// 重量
						}else{
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 END
							d才数 = 0;
							d重量 = 0;
							if(s運賃才数.Length > 0){
								try{
									d才数 = Decimal.Parse(s運賃才数);
								}catch(Exception){}
							}
							if(s運賃重量.Length > 0){
								try{
									d重量 = Decimal.Parse(s運賃重量);
								}catch(Exception){}
							}
							sbData.Append(sKanma + d才数.ToString());		// 才数
							sbData.Append(sKanma + d重量.ToString());		// 重量
// MOD 2011.04.13 東都）高木 重量入力不可対応 END
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 START
						}
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 END
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(26).TrimEnd() + sDbl);	// 輸送指示１
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(27).TrimEnd() + sDbl);	// 輸送指示２
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(28).TrimEnd() + sDbl);	// 品名記事１
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(29).TrimEnd() + sDbl);	// 品名記事２
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(30).TrimEnd() + sDbl);	// 品名記事３

						if(reader.GetString(25).Trim() == "0"){
							sbData.Append(sKanma + sDbl + sDbl);										// 指定日
						}else{
							sbData.Append(sKanma + sDbl + reader.GetString(25).Trim() + sDbl       );	// 指定日
						}

						sbData.Append(sKanma + sDbl + sSng + reader.GetString(33).Trim() + sDbl);	// お客様出荷番号
						sbData.Append(sKanma + sDbl + sDbl);										// 予備
						sbData.Append(sKanma + sDbl + reader.GetString(31).Trim() + sDbl       );	// 元着区分
						sbData.Append(sKanma + reader.GetString(32).Trim()                     );	// 保険金額
						sbData.Append(sKanma + sDbl + reader.GetString(1).Trim() + sDbl);	// 出荷日
						sbData.Append(sKanma + sDbl + sDbl);								// 登録日（省略）
					}else{
						sbData.Append(sDbl + reader.GetString(0).Trim() + sDbl);					// 登録日
						sbData.Append(sKanma + sDbl + reader.GetString(1).Trim() + sDbl       );	// 出荷日
						string sNo = reader.GetString(2).Trim();									// 送り状番号(XXX-XXXX-XXXX)
						if(sNo.Length == 15){
							sbData.Append(sKanma + sDbl + sNo.Substring(4,3)
								+ "-" + sNo.Substring(7,4) + "-" + sNo.Substring(11) + sDbl);
						}else{
							sbData.Append(sKanma + sDbl + " " + sDbl);
						}
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(3).Trim() + sDbl);	// 荷受人ＣＤ
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(4).Trim() + sDbl);	// 荷受人郵便番号
						sbData.Append(sKanma + sDbl + reader.GetString(5).Trim() + sDbl);			// 荷受人電話番号
						sbData.Append(sKanma + sDbl + reader.GetString(6).Trim() + sDbl);			// 荷受人住所１
						sbData.Append(sKanma + sDbl + reader.GetString(7).Trim() + sDbl);			// 荷受人住所２
						sbData.Append(sKanma + sDbl + reader.GetString(8).Trim() + sDbl);			// 荷受人住所３
						sbData.Append(sKanma + sDbl + reader.GetString(9).Trim() + sDbl);			// 荷受人名前１
						sbData.Append(sKanma + sDbl + reader.GetString(10).Trim() + sDbl);			// 荷受人名前２
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(11).Trim() + sDbl);	// 特殊計
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(12).Trim() + sDbl);	// 着店ＣＤ
						sbData.Append(sKanma + sDbl + reader.GetString(13).Trim() + sDbl       );	// 着店名
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(14).Trim() + sDbl);	// 荷送人ＣＤ
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(15).Trim() + sDbl);	// 荷送人郵便番号

						string sTel = reader.GetString(16).Trim();									// 荷送人電話番号
						if(sTel.Length != 0){
							sbData.Append(sKanma + sDbl + "(" + sTel + ")"
								+ "-" + reader.GetString(17).Trim() + "-" + reader.GetString(18).Trim() + sDbl);
						}else{
							sbData.Append(sKanma + sDbl + " " + sDbl);
						}

						sbData.Append(sKanma + sDbl + reader.GetString(19).Trim() + sDbl);			// 荷送人住所１
						sbData.Append(sKanma + sDbl + reader.GetString(20).Trim() + sDbl);			// 荷送人住所２
						sbData.Append(sKanma + sDbl + reader.GetString(21).Trim() + sDbl);			// 荷送人名前１
						sbData.Append(sKanma + sDbl + reader.GetString(22).Trim() + sDbl);			// 荷送人名前２
						sbData.Append(sKanma + reader.GetString(23)                            );	// 個数
// MOD 2011.04.13 東都）高木 重量入力不可対応 START
//						d才数 = reader.GetDecimal(36);												// 才数
//						d才数 = d才数 * 8;
//						if(d才数 == 0){
//							sbData.Append(sKanma + reader.GetString(24)                            );	// 重量
//						}else{
//							sbData.Append(sKanma + d才数.ToString()                            );
//						}
						s運賃才数 = reader.GetString(40).TrimEnd();
						s運賃重量 = reader.GetString(41).TrimEnd();
						d才数重量 = 0;
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 START
						s重量入力制御 = reader.GetString(42).TrimEnd();
						if(s重量入力制御 == "1"
						&& s運賃才数.Length == 0 && s運賃重量.Length == 0
//						&& (reader.GetString(24).TrimEnd() != "0" || reader.GetDecimal(36) != 0)
						){
							d才数重量 += (reader.GetDecimal(36) * 8);		// 才数
							if(reader.GetString(24).TrimEnd().Length > 0){	// 重量
								try{
									d才数重量 += Decimal.Parse(reader.GetString(24).TrimEnd());
								}catch(Exception){}
							}
							sbData.Append(sKanma + d才数重量.ToString());	// 重量
						}else{
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 END
							if(s運賃才数.Length > 0){
								try{
									d才数重量 += (Decimal.Parse(s運賃才数) * 8);
								}catch(Exception){}
							}
							if(s運賃重量.Length > 0){
								try{
									d才数重量 += Decimal.Parse(s運賃重量);
								}catch(Exception){}
							}
							sbData.Append(sKanma + d才数重量.ToString());		// 重量
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 START
						}
// MOD 2011.05.06 東都）高木 お客様ごとに重量入力制御 END
// MOD 2011.04.13 東都）高木 重量入力不可対応 END
						if(reader.GetString(25).Trim() == "0"){
							sbData.Append(sKanma + sDbl + sDbl);										// 指定日
						}else{
							sbData.Append(sKanma + sDbl + reader.GetString(25).Trim() + sDbl       );	// 指定日
						}
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(26).TrimEnd() + sDbl);	// 輸送指示１
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(27).TrimEnd() + sDbl);	// 輸送指示２
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(28).TrimEnd() + sDbl);	// 品名記事１
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(29).TrimEnd() + sDbl);	// 品名記事２
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(30).TrimEnd() + sDbl);	// 品名記事３
						sbData.Append(sKanma + sDbl + reader.GetString(31).Trim() + sDbl       );	// 元着区分
						sbData.Append(sKanma + reader.GetString(32)                            );	// 保険金額
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(33).Trim() + sDbl);	// お客様出荷番号
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(34).Trim() + sDbl);	// 得意先ＣＤ
						sbData.Append(sKanma + sDbl + sSng + reader.GetString(35).Trim() + sDbl);	// 部課ＣＤ
					}
// MOD 2014.03.20 BEVAS）高杉 ＣＳＶ出力に配完日付・時刻を追加 START
					if(s配完Ｓ出力形式.Equals("1"))
					{
						sbData.Append(sKanma + sDbl + reader.GetString(43).Trim() + sDbl       );	// 配完日付・時刻
					}
					sList.Add(sbData);
// MOD 2014.03.20 BEVAS）高杉 ＣＳＶ出力に配完日付・時刻を追加 END
				}

//				sbQuery2 = new StringBuilder(1024);
//				sbQuery2.Append(GET_CSVWRITE3_SELECT);
//				sbQuery2.Append(GET_CSVWRITE3_FROM_2);
//				sbQuery2.Append(sbQuery);
//				if(s日付区分 == "0"){
//					sbQuery2.Append(GET_CSVWRITE3_SORT_1);
//				}else{
//					sbQuery2.Append(GET_CSVWRITE3_SORT_2);
//				}
//				reader = CmdSelect(sUser, conn2, sbQuery2);
//
//				while (reader.Read()){
//					sbData = new StringBuilder(1024);
//					if(sＣＳＶ出力形式.Equals("1")){
//						sbData.Append(sDbl + sSng + reader.GetString(3).Trim() + sDbl);		// 荷受人ＣＤ
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(5).Trim() + sDbl);	// 荷受人電話番号
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(6).Trim() + sDbl);	// 荷受人住所１
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(7).Trim() + sDbl);	// 荷受人住所２
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(8).Trim() + sDbl);	// 荷受人住所３
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(9).Trim() + sDbl);	// 荷受人名前１
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(10).Trim() + sDbl);	// 荷受人名前２
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(4).Trim() + sDbl);	// 荷受人郵便番号
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(11).Trim() + sDbl);	// 特殊計
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(12).Trim() + sDbl);	// 着店ＣＤ
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(14).Trim() + sDbl);	// 荷送人ＣＤ
//						sbData.Append(sKanma + reader.GetString(23).Trim()                     );	// 個数
//						sbData.Append(sKanma + reader.GetDecimal(36).ToString().Trim()         );	// 才数
//						sbData.Append(sKanma + reader.GetString(24).Trim()                     );	// 重量
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(26).TrimEnd() + sDbl);	// 輸送指示１
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(27).TrimEnd() + sDbl);	// 輸送指示２
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(28).TrimEnd() + sDbl);	// 品名記事１
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(29).TrimEnd() + sDbl);	// 品名記事２
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(30).TrimEnd() + sDbl);	// 品名記事３
//
//						if(reader.GetString(25).Trim() == "0"){
//							sbData.Append(sKanma + sDbl + sDbl);										// 指定日
//						}else{
//							sbData.Append(sKanma + sDbl + reader.GetString(25).Trim() + sDbl       );	// 指定日
//						}
//
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(33).Trim() + sDbl);	// お客様出荷番号
//						sbData.Append(sKanma + sDbl + sDbl);										// 予備
//						sbData.Append(sKanma + sDbl + reader.GetString(31).Trim() + sDbl       );	// 元着区分
//						sbData.Append(sKanma + reader.GetString(32).Trim()                     );	// 保険金額
//						sbData.Append(sKanma + sDbl + reader.GetString(1).Trim() + sDbl);	// 出荷日
//						sbData.Append(sKanma + sDbl + sDbl);								// 登録日（省略）
//					}else{
//						sbData.Append(sDbl + reader.GetString(0).Trim() + sDbl);					// 登録日
//						sbData.Append(sKanma + sDbl + reader.GetString(1).Trim() + sDbl       );	// 出荷日
//						string sNo = reader.GetString(2).Trim();									// 送り状番号(XXX-XXXX-XXXX)
//						if(sNo.Length == 15){
//							sbData.Append(sKanma + sDbl + sNo.Substring(4,3)
//								+ "-" + sNo.Substring(7,4) + "-" + sNo.Substring(11) + sDbl);
//						}else{
//							sbData.Append(sKanma + sDbl + " " + sDbl);
//						}
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(3).Trim() + sDbl);	// 荷受人ＣＤ
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(4).Trim() + sDbl);	// 荷受人郵便番号
//						sbData.Append(sKanma + sDbl + reader.GetString(5).Trim() + sDbl);			// 荷受人電話番号
//						sbData.Append(sKanma + sDbl + reader.GetString(6).Trim() + sDbl);			// 荷受人住所１
//						sbData.Append(sKanma + sDbl + reader.GetString(7).Trim() + sDbl);			// 荷受人住所２
//						sbData.Append(sKanma + sDbl + reader.GetString(8).Trim() + sDbl);			// 荷受人住所３
//						sbData.Append(sKanma + sDbl + reader.GetString(9).Trim() + sDbl);			// 荷受人名前１
//						sbData.Append(sKanma + sDbl + reader.GetString(10).Trim() + sDbl);			// 荷受人名前２
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(11).Trim() + sDbl);	// 特殊計
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(12).Trim() + sDbl);	// 着店ＣＤ
//						sbData.Append(sKanma + sDbl + reader.GetString(13).Trim() + sDbl       );	// 着店名
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(14).Trim() + sDbl);	// 荷送人ＣＤ
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(15).Trim() + sDbl);	// 荷送人郵便番号
//
//						string sTel = reader.GetString(16).Trim();									// 荷送人電話番号
//						if(sTel.Length != 0){
//							sbData.Append(sKanma + sDbl + "(" + sTel + ")"
//								+ "-" + reader.GetString(17).Trim() + "-" + reader.GetString(18).Trim() + sDbl);
//						}else{
//							sbData.Append(sKanma + sDbl + " " + sDbl);
//						}
//
//						sbData.Append(sKanma + sDbl + reader.GetString(19).Trim() + sDbl);			// 荷送人住所１
//						sbData.Append(sKanma + sDbl + reader.GetString(20).Trim() + sDbl);			// 荷送人住所２
//						sbData.Append(sKanma + sDbl + reader.GetString(21).Trim() + sDbl);			// 荷送人名前１
//						sbData.Append(sKanma + sDbl + reader.GetString(22).Trim() + sDbl);			// 荷送人名前２
//						sbData.Append(sKanma + reader.GetString(23)                            );	// 個数
//
//						d才数 = reader.GetDecimal(36);												// 才数
//						d才数 = d才数 * 8;
//						if(d才数 == 0){
//							sbData.Append(sKanma + reader.GetString(24)                            );	// 重量
//						}else{
//							sbData.Append(sKanma + d才数.ToString()                            );
//						}
//						if(reader.GetString(25).Trim() == "0"){
//							sbData.Append(sKanma + sDbl + sDbl);										// 指定日
//						}else{
//							sbData.Append(sKanma + sDbl + reader.GetString(25).Trim() + sDbl       );	// 指定日
//						}
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(26).TrimEnd() + sDbl);	// 輸送指示１
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(27).TrimEnd() + sDbl);	// 輸送指示２
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(28).TrimEnd() + sDbl);	// 品名記事１
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(29).TrimEnd() + sDbl);	// 品名記事２
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(30).TrimEnd() + sDbl);	// 品名記事３
//						sbData.Append(sKanma + sDbl + reader.GetString(31).Trim() + sDbl       );	// 元着区分
//						sbData.Append(sKanma + reader.GetString(32)                            );	// 保険金額
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(33).Trim() + sDbl);	// お客様出荷番号
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(34).Trim() + sDbl);	// 得意先ＣＤ
//						sbData.Append(sKanma + sDbl + sSng + reader.GetString(35).Trim() + sDbl);	// 部課ＣＤ
//					}
//					sList.Add(sbData);
//				}

				disposeReader(reader);
				reader = null;

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0){
					sRet[0] = "該当データがありません";
				}else{
					sRet[0] = "正常終了";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext()){
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
			}catch (OracleException ex){
				sRet[0] = chgDBErrMsg(sUser, ex);
			}catch (Exception ex){
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}finally{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}
// MOD 2010.04.30 東都）高木 ＣＳＶ出力機能の追加 END
// MOD 2010.10.12 東都）高木 運賃エラー対応 START
		/*********************************************************************
		 * 運賃エラー件数確認
		 * 引数：店所ＣＤ、出荷日開始、出荷日終了、件数
		 * 戻値：ステータス、出荷日、件数、...
		 *
		 *********************************************************************/
		[WebMethod]
		public String[] Get_UntinErrCntChk(string[] sUser, string[] sData)
		{
			logWriter(sUser, INF, "運賃エラー件数確認開始");
			string s発店ＣＤ   = sData[0];
			string s出荷日開始 = (sData.Length > 1) ? sData[1] : "";
			string s出荷日終了 = (sData.Length > 2) ? sData[2] : "";
			string s件数       = (sData.Length > 3) ? sData[3] : "5";
			int i件数 = 5;
			try{
				i件数 = int.Parse(s件数);
			}catch(Exception){
			}
			OracleConnection conn2 = null;
			string[] sRet = new string[1+(i件数*2)];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null){
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}

			try{
				StringBuilder sbQuery = new StringBuilder(1024);
				OracleDataReader reader;
				sbQuery.Append("SELECT 出荷日, COUNT(ROWID)  \n");
				sbQuery.Append(" FROM \"ＳＴ０１出荷ジャーナル\" \n");
				sbQuery.Append(" WHERE 発店ＣＤ = '" + s発店ＣＤ + "' \n");
				if(s出荷日開始.Length > 0){
					sbQuery.Append(" AND 出荷日 >= '" + s出荷日開始 + "' \n");
				}
				if(s出荷日終了.Length > 0){
					sbQuery.Append(" AND 出荷日 <= '" + s出荷日終了 + "' \n");
				}
				sbQuery.Append(" AND ( 運賃 > 0 OR 中継 > 0 OR 諸料金 > 0 ) \n");
				sbQuery.Append(" AND \"運賃エラー確認ＦＧ\" = ' ' \n");
				sbQuery.Append(" AND 削除ＦＧ = '1' \n");
				sbQuery.Append(" GROUP BY 出荷日 \n");
				sbQuery.Append(" ORDER BY 出荷日 DESC \n");

				reader = CmdSelect(sUser, conn2, sbQuery);
				int iPos = 1;
				while(reader.Read() && iPos < sRet.Length){
					sRet[iPos  ] = reader.GetString(0).TrimEnd();	// 出荷日
					sRet[iPos+1] = reader.GetDecimal(1).ToString(); // 件数
					iPos+=2;
				}
				disposeReader(reader);
				reader = null;
				if(sRet[1] == null){
					sRet[0] = "該当なし";
				}else{
					sRet[0] = "正常終了";
				}
				logWriter(sUser, INF, sRet[0]);
			}catch (OracleException ex){
				sRet[0] = chgDBErrMsg(sUser, ex);
			}catch (Exception ex){
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}finally{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}
		/*********************************************************************
		 * 運賃エラー確認ＦＧ更新
		 * 引数：会員ＣＤ、部門ＣＤ、登録日、ジャーナルＮＯ、運賃エラー確認ＦＧ
		 * 戻値：ステータス
		 *
		 *********************************************************************/
		[WebMethod]
		public String[] Upd_UntinErrKakuninFG(string[] sUser, string[] sData)
		{
			logWriter(sUser, INF, "運賃エラー確認ＦＧ更新開始");
			string s会員ＣＤ   = sData[0];
			string s部門ＣＤ   = sData[1];
			string s登録日     = sData[2];
			string sジャＮＯ   = sData[3];
			string s確認ＦＧ   = sData[4];

			OracleConnection conn2 = null;
			string[] sRet = new string[2];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null){
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}

			OracleTransaction tran;
			tran = conn2.BeginTransaction();
			try{
				StringBuilder sbQuery = new StringBuilder(1024);
				sbQuery.Append("UPDATE \"ＳＴ０１出荷ジャーナル\" \n");
				sbQuery.Append(" SET \"運賃エラー確認ＦＧ\" = '" + s確認ＦＧ + "' ");
				sbQuery.Append(" WHERE 会員ＣＤ = '" + s会員ＣＤ + "' \n");
				sbQuery.Append(" AND 部門ＣＤ = '" + s部門ＣＤ + "' \n");
				sbQuery.Append(" AND 登録日 = '" + s登録日 + "' \n");
				sbQuery.Append(" AND \"ジャーナルＮＯ\" = " + sジャＮＯ + " \n");

				if (CmdUpdate(sUser, conn2, sbQuery) != 0){
					tran.Commit();
					sRet[0] = "正常終了";
				}else{
					tran.Rollback();
					sRet[0] = "他の端末で更新されています";
				}
				logWriter(sUser, INF, sRet[0]);
			}catch (OracleException ex){
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}catch (Exception ex){
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}finally{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}
// MOD 2010.10.12 東都）高木 運賃エラー対応 END
// MOD 2010.11.19 東都）高木 配完情報などの取得 START
		/*********************************************************************
		 * 出荷情報検索
		 * 引数：[会員ＣＤ, 部門ＣＤ, 登録日, ジャーナルＮＯ, 送り状番号]の配列
		 * 戻値：ステータス、更新日時
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Get_SyukkaEtc(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "出荷等ＣＳＶ出力用取得開始");

			OracleConnection conn2 = null;
			string[] sRet            = new string[sKey.Length + 1];
			string[] s会員ＣＤ       = new string[sKey.Length];
			string[] s部門ＣＤ       = new string[sKey.Length];
			string[] s登録日         = new string[sKey.Length];
			string[] sジャーナルＮＯ = new string[sKey.Length];
			string[] s送り状番号     = new string[sKey.Length];

			int i件数 = 0;
			string[] sData;
			while(i件数 < sKey.Length){
				sData = sKey[i件数].Split(',');
				if(sData.Length < 5) break;
				s会員ＣＤ      [i件数] = sData[0];
				s部門ＣＤ      [i件数] = sData[1];
				s登録日        [i件数] = sData[2];
				sジャーナルＮＯ[i件数] = sData[3];
				s送り状番号    [i件数] = sData[4];
				sData = null;
				i件数++;
			}
			if(i件数 == 0){
				sRet[0] = "引数エラー";
				return sRet;
			}

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null){
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}

			string cmdQuery = "";
			try{
				OracleDataReader reader;
				OracleDataReader reader2;
				int iRow = 0;
				for(int iCnt = 0; iCnt < i件数; iCnt++){
					iRow++;
					sRet[iRow] = "";
					sRet[iRow] += "出荷データ";
					cmdQuery
						= "SELECT * FROM \"ＳＴ０１出荷ジャーナル\" \n"
						+ " WHERE ( \n"
						+ " 会員ＣＤ = '" + s会員ＣＤ[iCnt] + "' \n"
						+ " AND 部門ＣＤ = '" + s部門ＣＤ[iCnt] + "' \n"
						+ " AND 登録日 = '" + s登録日[iCnt] + "' \n"
						+ " AND \"ジャーナルＮＯ\" = " + sジャーナルＮＯ[iCnt] + " \n"
						+ " ) \n"
						;

					reader = CmdSelect(sUser, conn2, cmdQuery);
					if(reader.Read()){
						for(int iCntCol = 0; iCntCol < reader.FieldCount; iCntCol++){
							if(reader.GetValue(iCntCol) is System.String){
								sRet[iRow] += "," + reader.GetString(iCntCol).TrimEnd();
							}else{
								sRet[iRow] += "," + reader.GetDecimal(iCntCol).ToString();
							}
						}
					}else{
						for(int iCntCol = 0; iCntCol < reader.FieldCount; iCntCol++){
							sRet[iRow] += ",";
						}
					}
					disposeReader(reader);
					reader = null;
					if(s送り状番号[iCnt].Length > 0){
						sRet[iRow] += ",配完情報";
						cmdQuery
							= "SELECT * FROM ＧＴ０２配完 \n"
							+ " WHERE 原票番号 = '0000" + s送り状番号[iCnt] + "' \n"
							;
						reader2 = CmdSelect(sUser, conn2, cmdQuery);
						if(reader2.Read()){
							for(int iCntCol = 0; iCntCol < reader2.FieldCount; iCntCol++){
								if(reader2.GetValue(iCntCol) is System.String){
									sRet[iRow] += "," + reader2.GetString(iCntCol).TrimEnd();
								}else{
									sRet[iRow] += "," + reader2.GetDecimal(iCntCol).ToString();
								}
							}
						}else{
							for(int iCntCol = 0; iCntCol < reader2.FieldCount; iCntCol++){
								sRet[iRow] += ",";
							}
						}
						disposeReader(reader2);
						reader2 = null;

						sRet[iRow] += ",運賃情報";
						cmdQuery
							= "SELECT * FROM ＧＴ０３原票運賃 \n"
							+ " WHERE 原票番号 = '0000" + s送り状番号[iCnt] + "' \n"
							;
						reader2 = CmdSelect(sUser, conn2, cmdQuery);
						if(reader2.Read()){
							for(int iCntCol = 0; iCntCol < reader2.FieldCount; iCntCol++){
								if(reader2.GetValue(iCntCol) is System.String){
									sRet[iRow] += "," + reader2.GetString(iCntCol).TrimEnd();
								}else{
									sRet[iRow] += "," + reader2.GetDecimal(iCntCol).ToString();
								}
							}
						}else{
							for(int iCntCol = 0; iCntCol < reader2.FieldCount; iCntCol++){
								sRet[iRow] += ",";
							}
						}
						disposeReader(reader2);
						reader2 = null;
					}
				}
				if(iRow == 0){
					sRet[0] = "該当データがありません";
				}else{
					sRet[0] = "正常終了";
				}
				logWriter(sUser, INF, sRet[0]);
			}catch (OracleException ex){
				sRet[0] = chgDBErrMsg(sUser, ex);
			}catch (Exception ex){
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}finally{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}
// MOD 2010.11.19 東都）高木 配完情報などの取得 END
// MOD 2011.02.02 東都）高木 出荷データ出荷日範囲取得 START
		/*********************************************************************
		 * 出荷データ出荷日範囲取得
		 * 引数：[会員ＣＤ, 部門ＣＤ, 登録日, ジャーナルＮＯ, 送り状番号]の配列
		 * 戻値：ステータス、更新日時
		 *
		 *********************************************************************/
		private static string GET_SYUKKABIMINMAX_SELECT1
			= "SELECT MIN(出荷日), MAX(出荷日) \n"
			+ " FROM \"ＳＴ０１出荷ジャーナル\" \n"
			+ " WHERE 削除ＦＧ = '0' \n"
			;
		private static string GET_SYUKKABIMINMAX_SELECT2
			= "SELECT MIN(出荷日), MAX(出荷日) \n"
			+ " FROM \"ＳＴ０２出荷履歴\" \n"
			+ " WHERE 削除ＦＧ = '0' \n"
			;
		[WebMethod]
		public string[] Get_SyukkaBiMinMax(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "出荷データ出荷日範囲取得開始");

			string s会員ＣＤ = (sKey.Length > 0) ? sKey[0] : "";
			string s部門ＣＤ = (sKey.Length > 1) ? sKey[1] : "";

			OracleConnection conn2 = null;
			string[] sRet = new string[]{"","","","",""};

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null){
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}

			StringBuilder sbQuery = new StringBuilder(1024);
			try{
				OracleDataReader reader;
				sbQuery.Append(GET_SYUKKABIMINMAX_SELECT1);
				if(s会員ＣＤ.Length > 0){
					sbQuery.Append(" AND 会員ＣＤ = '" + s会員ＣＤ + "' \n");
				}
				if(s部門ＣＤ.Length > 0){
					sbQuery.Append(" AND 部門ＣＤ = '" + s部門ＣＤ + "' \n");
				}
				reader = CmdSelect(sUser, conn2, sbQuery);
				if(reader.Read()){
					sRet[1] = reader.GetString(0).TrimEnd();
					sRet[2] = reader.GetString(1).TrimEnd();
				}
				disposeReader(reader);
				reader = null;

				sbQuery = new StringBuilder(1024);
				sbQuery.Append(GET_SYUKKABIMINMAX_SELECT2);
				if(s会員ＣＤ.Length > 0){
					sbQuery.Append(" AND 会員ＣＤ = '" + s会員ＣＤ + "' \n");
				}
				if(s部門ＣＤ.Length > 0){
					sbQuery.Append(" AND 部門ＣＤ = '" + s部門ＣＤ + "' \n");
				}
				reader = CmdSelect(sUser, conn2, sbQuery);
				if(reader.Read()){
					sRet[3] = reader.GetString(0).TrimEnd();
					sRet[4] = reader.GetString(1).TrimEnd();
				}
				disposeReader(reader);
				reader = null;

				if(sRet[1].Length == 0){
					sRet[0] = "該当出荷データがありません";
				}else if(sRet[3].Length == 0){
					sRet[0] = "該当出荷履歴データがありません";
				}else{
					sRet[0] = "正常終了";
				}
				logWriter(sUser, INF, sRet[0]);
			}catch (OracleException ex){
				sRet[0] = chgDBErrMsg(sUser, ex);
			}catch (Exception ex){
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}finally{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}
// MOD 2011.02.02 東都）高木 出荷データ出荷日範囲取得 END

// ADD 2015.11.24 bevas）松本 出荷実績表印刷機能追加(is-2管理) START
		/*********************************************************************
		 * 出荷実績印刷データ取得
		 * 引数：会員ＣＤ、部門ＣＤ、出荷日 or 登録日、
		 *		 開始日、終了日
		 * 戻値：ステータス、登録日、荷受人ＣＤ...
		 *
		 *********************************************************************/
		private static string GET_SYUKKA_SELECT_4
			= "SELECT \n"
			+	"  S.登録日 \n"
			+	", S.出荷日 \n"
			+	", S.送り状番号 \n"
			+	", S.荷受人ＣＤ \n"
			+	", S.郵便番号 \n"
			+	", S.電話番号１ \n"
			+	", S.電話番号２ \n"
			+	", S.電話番号３ \n"
			+	", S.住所１ \n"
			+	", S.住所２ \n"
			+	", S.住所３ \n"
			+	", S.名前１ \n"
			+	", S.名前２ \n"
			+	", S.着店ＣＤ \n"
			+	", S.着店名 \n"
			+	", S.荷送人ＣＤ \n"
			+	", NVL(SM01.郵便番号, ' ') \n"
			+	", NVL(SM01.電話番号１, ' ') \n"
			+	", NVL(SM01.電話番号２, ' ') \n"
			+	", NVL(SM01.電話番号３, ' ') \n"
			+	", NVL(SM01.住所１, ' ') \n"
			+	", NVL(SM01.住所２, ' ') \n"
			+	", NVL(SM01.名前１, ' ') \n"
			+	", NVL(SM01.名前２, ' ') \n"
			+	", S.荷送人部署名 \n"
			+	", TO_CHAR(S.個数) \n"
			+	", TO_CHAR(S.重量) \n"
			+	", S.指定日 \n"
			+	", S.輸送指示１ \n"
			+	", S.輸送指示２ \n"
			+	", S.品名記事１ \n"
			+	", S.品名記事２ \n"
			+	", S.品名記事３ \n"
			+	", S.元着区分 \n"
			+	", TO_CHAR(S.保険金額) \n"
			+	", S.お客様出荷番号 \n"
			+	", TO_CHAR(S.才数) \n"
			+	", S.指定日区分 \n"
			+	", TO_CHAR(S.運賃 + S.中継) \n"
			+	", NVL(CM01.記事連携ＦＧ, '0') \n"
			+	", S.得意先ＣＤ \n"
			+	", S.部課ＣＤ \n"
			+	", S.部課名 \n"
			+	", S.運賃才数 \n"
			+	", S.運賃重量 \n"
			+	", NVL(CM01.保留印刷ＦＧ, '0') \n"
			+	", S.品名記事４ \n"
			+	", S.品名記事５ \n"
			+	", S.品名記事６ \n"
			+	", S.処理０３ \n"
			//is-2管理側での新規追加分
			+	", S.会員ＣＤ \n"
			+	", S.部門ＣＤ \n"
			+	", CM01.会員名 \n"
			+	", S.\"ジャーナルＮＯ\" \n"
			;

		private static string GET_PUBLISHEDPRINT4_FROM_1
			= " FROM \"ＳＴ０１出荷ジャーナル\" ST01 , ＳＭ０１荷送人 SM01 \n"
			+ ", ＣＭ０１会員 CM01 \n"
			;

		private static string GET_PUBLISHEDPRINT4_FROM_2 
			= " FROM \"ＳＴ０２出荷履歴\"       ST02 , ＳＭ０１荷送人 SM01 \n"
			+ ", ＣＭ０１会員 CM01 \n"
			;

		private static string GET_PUBLISHEDPRINT4_SORT_1
			= " ORDER BY S.出荷日, S.荷送人ＣＤ, S.会員ＣＤ, S.部門ＣＤ, S.登録日, S.\"ジャーナルＮＯ\" "
			;

		private static string GET_PUBLISHEDPRINT4_SORT_2
			= " ORDER BY           S.荷送人ＣＤ, S.会員ＣＤ, S.部門ＣＤ, S.登録日, S.\"ジャーナルＮＯ\" "
			;

		[WebMethod]
		public ArrayList Get_PublishedPrintData4(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "出荷実績印刷データ４開始");

			string s会員ＣＤ   = sKey[0];
			string s部門ＣＤ   = sKey[1];
			string s荷送人ＣＤ = sKey[2];
			string s日付区分   = sKey[3];
			string s開始日     = sKey[4];
			string s終了日     = sKey[5];
			string s状態ＣＤ   = sKey[6];
			bool   b未出荷     = sKey[6].Equals("90");
			string s送り状番号 = sKey[7];
			string s削除ＦＧ   = sKey[8];
			string s発店ＣＤ   = sKey[9];

			OracleConnection conn2 = null;
			ArrayList alRet = new ArrayList();

			string[] sRet = new string[1];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "ＤＢ接続エラー";
				alRet.Add(sRet);
				return alRet;
			}

			string  s運賃才数 = "";
			string  s運賃重量 = "";
			decimal d才数 = 0;
			decimal d重量 = 0;
			string  s重量入力制御 = "0";

			StringBuilder sbQuery = new StringBuilder(1024);
			StringBuilder sbQuery2 = new StringBuilder(1024);
			try
			{
				if(s送り状番号.Length == 0)
				{
					/** (1) 検索条件に[送り状番号]が入力されていない場合 */
					if(b未出荷)
					{
						/** (1-1-1) [輸送状況]が[未出荷]の場合 */
						sbQuery.Append(", ＳＴ０５未出荷分 ST05 \n");
						// 会員ＣＤ
						if(s会員ＣＤ.Length > 0)
						{
							sbQuery.Append(" WHERE ST05.会員ＣＤ = '" + s会員ＣＤ + "' \n");
						}
						else
						{
							sbQuery.Append(" WHERE ST05.会員ＣＤ > ' ' \n");
						}

						// 部門ＣＤ
						if(s部門ＣＤ.Length > 0)
						{
							sbQuery.Append(" AND ST05.部門ＣＤ = '" + s部門ＣＤ + "' \n");
						}

						// 出荷日／登録日
						if(s日付区分 == "0")
						{
							// [出荷日]が[出荷日]の場合
							sbQuery.Append(" AND ST05.出荷日  BETWEEN '"+ s開始日 + "' AND '"+ s終了日 +"' \n");
						}
						else
						{
							// [出荷日]が[登録日]の場合
							sbQuery.Append(" AND ST05.登録日  BETWEEN '"+ s開始日 + "' AND '"+ s終了日 +"' \n");
						}

						sbQuery.Append(" AND ST05.出荷日 < TO_CHAR(SYSDATE,'YYYYMMDD') \n");
						sbQuery.Append(" AND ST05.状態   = '02' \n");

						// 発店ＣＤ
						if(s発店ＣＤ.Length > 0)
						{
							sbQuery.Append(" AND ST05.発店ＣＤ = '"+ s発店ＣＤ + "' \n");
						}

						// 各テーブル同士の結合条件
						sbQuery.Append(" AND ST05.会員ＣＤ = S.会員ＣＤ \n");
						sbQuery.Append(" AND ST05.部門ＣＤ = S.部門ＣＤ \n");
						sbQuery.Append(" AND ST05.登録日 = S.登録日 \n");
						sbQuery.Append(" AND ST05.\"ジャーナルＮＯ\" = S.\"ジャーナルＮＯ\" \n");
					}
					else
					{
						/** (1-1-2) [輸送状況]が[未出荷]以外の場合 */
						// 会員ＣＤ
						if(s会員ＣＤ.Length > 0)
						{
							sbQuery.Append(" WHERE S.会員ＣＤ = '" + s会員ＣＤ + "' \n");
						}
						else
						{
							sbQuery.Append(" WHERE S.会員ＣＤ > ' ' \n");
						}

						// 部門ＣＤ
						if(s部門ＣＤ.Length > 0)
						{
							sbQuery.Append(" AND S.部門ＣＤ = '" + s部門ＣＤ + "' \n");
						}
					}

					/** (1-2) 共通検索条件(送り状番号の条件なし) */
					// 荷送人ＣＤ
					if(s荷送人ＣＤ.Length > 0)
					{
						sbQuery.Append(" AND S.荷送人ＣＤ = '"+ s荷送人ＣＤ + "' \n");
					}

					// 出荷日／登録日
					if(s日付区分 == "0")
					{
						// [出荷日]が[出荷日]の場合
						sbQuery.Append(" AND S.出荷日  BETWEEN '"+ s開始日 + "' AND '"+ s終了日 +"' \n");
					}
					else
					{
						// [出荷日]が[登録日]の場合
						sbQuery.Append(" AND S.登録日  BETWEEN '"+ s開始日 + "' AND '"+ s終了日 +"' \n");
					}

					// 状態
					if(s状態ＣＤ == "00")
					{
						// [輸送状況]が[未発行]の場合、何もしない
						;
					}
					else
					{
						// [輸送状況]が[未発行]以外の場合
						if(b未出荷)
						{
							// [輸送状況]が[未出荷]の場合
							sbQuery.Append(" AND S.出荷日 < TO_CHAR(SYSDATE,'YYYYMMDD') \n");
							sbQuery.Append(" AND S.状態   = '02' \n");
						}
						else
						{
							// [輸送状況]が[未出荷]以外の場合
							sbQuery.Append(" AND S.状態 = '"+ s状態ＣＤ + "' \n");
						}
					}
				}
				else
				{
					/** (2) 検索条件に[送り状番号]が入力されていた場合 */
					// 送り状番号
					sbQuery.Append(" WHERE S.送り状番号 = '"+ s送り状番号 + "' \n");
				}

				/** (3) 共通検索条件 */
				// 削除ＦＧ
				if(s削除ＦＧ != "0")
				{
					if(s削除ＦＧ == "1")
					{
						// [状態]が[削除]の場合
						sbQuery.Append(" AND S.削除ＦＧ = '1' \n");
					}
					else
					{
						// [状態]が[出荷分]の場合
						sbQuery.Append(" AND S.削除ＦＧ = '0' \n");
					}
				}

				// 発店ＣＤ
				if(s発店ＣＤ.Length > 0)
				{
					sbQuery.Append(" AND S.発店ＣＤ = '"+ s発店ＣＤ + "' \n");
				}

				// 各テーブル同士の結合条件
				sbQuery.Append(" AND S.会員ＣＤ   = SM01.会員ＣＤ(+) \n");
				sbQuery.Append(" AND S.部門ＣＤ   = SM01.部門ＣＤ(+) \n");
				sbQuery.Append(" AND S.荷送人ＣＤ = SM01.荷送人ＣＤ(+) \n");
				sbQuery.Append(" AND S.会員ＣＤ   = CM01.会員ＣＤ(+) \n");

				OracleDataReader reader;
				sbQuery2.Append("SELECT * FROM ( \n");
				sbQuery2.Append(GET_SYUKKA_SELECT_4).Replace("S.","ST01.");
				sbQuery2.Append(GET_PUBLISHEDPRINT4_FROM_1);
				sbQuery2.Append(sbQuery).Replace("S.","ST01.");
				sbQuery2.Append(" UNION \n");
				sbQuery2.Append(GET_SYUKKA_SELECT_4).Replace("S.","ST02.");
				sbQuery2.Append(GET_PUBLISHEDPRINT4_FROM_2);
				sbQuery2.Append(sbQuery).Replace("S.","ST02.");
				sbQuery2.Append(") S \n");

				// ソートキーの設定
				if(s日付区分 == "0")
				{
					// [出荷日]が[出荷日]の場合
					sbQuery2.Append(GET_PUBLISHEDPRINT4_SORT_1);
				}
				else
				{
					// [出荷日]が[登録日]の場合
					sbQuery2.Append(GET_PUBLISHEDPRINT4_SORT_2);
				}

				reader = CmdSelect(sUser, conn2, sbQuery2);
				while (reader.Read())
				{
					string[] sData = new string[50];
					for(int iCnt = 0; iCnt < 39; iCnt++)
					{
						sData[iCnt] = reader.GetString(iCnt);
					}

					// 記事連携ＦＧが「1」の場合、運賃は表示しない
					if(reader.GetString(39).Equals("1"))
					{
						sData[38] = "0";
					}

					sData[39] = reader.GetString(40);	//得意先ＣＤ
					sData[40] = reader.GetString(41);	//部課ＣＤ
					sData[41] = reader.GetString(42);	//部課名
					sData[42] = reader.GetString(43);   //運賃才数
					sData[43] = reader.GetString(44);   //運賃重量
					s運賃才数 = reader.GetString(43).TrimEnd();
					s運賃重量 = reader.GetString(44).TrimEnd();
					s重量入力制御 = reader.GetString(45).TrimEnd();
					if(s重量入力制御 == "1" && s運賃才数.Length == 0 && s運賃重量.Length == 0)
					{
						sData[42] = sData[36]; //運賃才数 = 才数
						sData[43] = sData[26]; //運賃重量 = 重量
					}
					else
					{
						d才数 = 0;
						d重量 = 0;
						if(s運賃才数.Length > 0)
						{
							try
							{
								d才数 = Decimal.Parse(s運賃才数);
							}
							catch(Exception){}
						}
						if(s運賃重量.Length > 0)
						{
							try
							{
								d重量 = Decimal.Parse(s運賃重量);
							}
							catch(Exception){}
						}
						sData[26] = d重量.ToString();	// 重量
						sData[36] = d才数.ToString();	// 才数
					}

					sData[44] = reader.GetString(46).TrimEnd(); // 品名記事４
					sData[45] = reader.GetString(47).TrimEnd(); // 品名記事５
					sData[46] = reader.GetString(48).TrimEnd(); // 品名記事６
					sData[47] = reader.GetString(49).Trim();	// 配完日付・時刻
					sData[48] = reader.GetString(50).Trim();	// 会員ＣＤ
					sData[49] = reader.GetString(52).Trim();	// 会員名

					alRet.Add(sData);
				}

				disposeReader(reader);
				reader = null;

				if (alRet.Count == 0)
				{
					// ヒット件数が0件の場合
					sRet[0] = "該当データがありません";
					alRet.Add(sRet);
				}
				else if(alRet.Count > 1000)
				{
					// ヒット件数が1000件を超える場合
					sRet[0] = "1000件オーバー";
					alRet = new ArrayList();
					alRet.Add(sRet);
				}
				else
				{
					// それ以外（＝ヒット件数が1000件以下）の場合
					sRet[0] = "正常終了";
					alRet.Insert(0, sRet);
				}
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
				alRet.Insert(0, sRet);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				alRet.Insert(0, sRet);
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			
			return alRet;
		}

		/*********************************************************************
		 * 出荷印刷データ取得
		 * 引数：会員ＣＤ、部門ＣＤ、登録日、ジャーナルＮＯ
		 * 戻値：ステータス、荷受人ＣＤ、電話番号、住所...
		 *
		 *********************************************************************/
// MOD 2016.04.08 bevas) 松本 社内伝票機能追加対応 START
		private static string GET_INVOICEPRINT_SELECT
			= "SELECT \n"
			+	"  S.荷受人ＣＤ \n"
			+	", S.電話番号１ \n"
			+	", S.電話番号２ \n"
			+	", S.電話番号３ \n"
			+	", S.住所１ \n"
			+	", S.住所２ \n"
			+	", S.住所３ \n"
			+	", S.名前１ \n"
			+	", S.名前２ \n"
			+	", S.出荷日 \n"
			+	", S.送り状番号 \n"
			+	", S.郵便番号 \n"
			+	", S.着店ＣＤ \n"
			+	", NVL(CM14.店所ＣＤ, S.発店ＣＤ) \n"
			+	", SM01.電話番号１ \n"
			+	", SM01.電話番号２ \n"
			+	", SM01.電話番号３ \n"
			+	", SM01.住所１ \n"
			+	", SM01.住所２ \n"
			+	", SM01.住所３ \n"
			+	", SM01.名前１ \n"
			+	", SM01.名前２ \n"
			+	", S.個数 \n"
			+	", S.重量 \n"
			+	", S.保険金額 \n"
			+	", S.指定日 \n"
			+	", S.輸送指示１ \n"
			+	", S.輸送指示２ \n"
			+	", S.品名記事１ \n"
			+	", S.品名記事２ \n"
			+	", S.品名記事３ \n"
			+	", S.元着区分 \n"
			+	", S.送り状発行済ＦＧ \n"
			+	", S.才数 \n"
			+	", S.荷送人部署名 \n"
			+	", S.お客様出荷番号 \n"
			+	", S.輸送指示ＣＤ１ \n"
			+	", S.輸送指示ＣＤ２ \n"
			+	", S.指定日区分 \n"
			+	", S.郵便番号 \n"
			+	", S.仕分ＣＤ \n"
			+	", NVL(CM10.店所名, S.発店名) \n"
			+	", S.出荷済ＦＧ \n"
			+	", SM01.郵便番号 \n"
			+	", NVL(CM01.保留印刷ＦＧ, '0') \n"
			+	", S.品名記事４ \n"
			+	", S.品名記事５ \n"
			+	", S.品名記事６ \n"
			+	", S.着店名 \n"
			+	", S.発店ＣＤ \n"
			+	", S.発店名 \n";

		private static string GET_INVOICEPRINT_FROM_1
			= " FROM \"ＳＴ０１出荷ジャーナル\" ST01 \n"
			+ " LEFT JOIN ＣＭ０２部門 CM02 \n"
			+ " ON  S.会員ＣＤ = CM02.会員ＣＤ \n"
			+ " AND S.部門ＣＤ = CM02.部門ＣＤ \n"
			+ " LEFT JOIN ＣＭ１４郵便番号 CM14 \n"
			+ " ON CM02.郵便番号 = CM14.郵便番号 \n"
			+ " LEFT JOIN ＣＭ１０店所 CM10 \n"
			+ " ON CM14.店所ＣＤ = CM10.店所ＣＤ \n"
			+ " LEFT JOIN ＣＭ０１会員 CM01 \n"
			+ " ON S.会員ＣＤ = CM01.会員ＣＤ\n"
			+ ", \"ＳＭ０１荷送人\" SM01 \n"
			;

		private static string GET_INVOICEPRINT_FROM_2 
			= " FROM \"ＳＴ０２出荷履歴\" ST02 \n"
			+ " LEFT JOIN ＣＭ０２部門 CM02 \n"
			+ " ON  S.会員ＣＤ = CM02.会員ＣＤ \n"
			+ " AND S.部門ＣＤ = CM02.部門ＣＤ \n"
			+ " LEFT JOIN ＣＭ１４郵便番号 CM14 \n"
			+ " ON CM02.郵便番号 = CM14.郵便番号 \n"
			+ " LEFT JOIN ＣＭ１０店所 CM10 \n"
			+ " ON CM14.店所ＣＤ = CM10.店所ＣＤ \n"
			+ " LEFT JOIN ＣＭ０１会員 CM01 \n"
			+ " ON S.会員ＣＤ = CM01.会員ＣＤ\n"
			+ ", \"ＳＭ０１荷送人\" SM01 \n"
			;
// MOD 2016.04.08 bevas) 松本 社内伝票機能追加対応 END

		[WebMethod]
		public String[] Get_InvoicePrintData(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "出荷印刷データ取得開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[46];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}

			decimal d才数 = 0;
			string s郵便番号 = "";
			StringBuilder sbQuery  = new StringBuilder(1024);
// MOD 2016.04.08 bevas) 松本 社内伝票機能追加対応 START
			StringBuilder sbQueryWhere = new StringBuilder(1024);
// MOD 2016.04.08 bevas) 松本 社内伝票機能追加対応 END
			try
			{
// MOD 2016.04.08 bevas) 松本 社内伝票機能追加対応 START
//				sbQuery.Append("SELECT ");
//				sbQuery.Append(" ST01.荷受人ＣＤ ");
//				sbQuery.Append(",ST01.電話番号１ ");
//				sbQuery.Append(",ST01.電話番号２ ");
//				sbQuery.Append(",ST01.電話番号３ ");
//				sbQuery.Append(",ST01.住所１ ");
//				sbQuery.Append(",ST01.住所２ ");
//				sbQuery.Append(",ST01.住所３ ");
//				sbQuery.Append(",ST01.名前１ ");
//				sbQuery.Append(",ST01.名前２ ");
//				sbQuery.Append(",ST01.出荷日 ");
//				sbQuery.Append(",ST01.送り状番号 ");
//				sbQuery.Append(",ST01.郵便番号 ");
//				sbQuery.Append(",ST01.着店ＣＤ ");
//				sbQuery.Append(",NVL(CM14.店所ＣＤ, ST01.発店ＣＤ) ");
//				sbQuery.Append(",SM01.電話番号１ ");
//				sbQuery.Append(",SM01.電話番号２ ");
//				sbQuery.Append(",SM01.電話番号３ ");
//				sbQuery.Append(",SM01.住所１ ");
//				sbQuery.Append(",SM01.住所２ ");
//				sbQuery.Append(",SM01.住所３ ");
//				sbQuery.Append(",SM01.名前１ ");
//				sbQuery.Append(",SM01.名前２ ");
//				sbQuery.Append(",ST01.個数 ");
//				sbQuery.Append(",ST01.重量 ");
//				sbQuery.Append(",ST01.保険金額 ");
//				sbQuery.Append(",ST01.指定日 ");
//				sbQuery.Append(",ST01.輸送指示１ ");
//				sbQuery.Append(",ST01.輸送指示２ ");
//				sbQuery.Append(",ST01.品名記事１ ");
//				sbQuery.Append(",ST01.品名記事２ ");
//				sbQuery.Append(",ST01.品名記事３ ");
//				sbQuery.Append(",ST01.元着区分 ");
//				sbQuery.Append(",ST01.送り状発行済ＦＧ ");
//				sbQuery.Append(",ST01.才数 \n");
//				sbQuery.Append(",ST01.荷送人部署名 ");
//				sbQuery.Append(",ST01.お客様出荷番号 ");
//				sbQuery.Append(",ST01.輸送指示ＣＤ１ ");
//				sbQuery.Append(",ST01.輸送指示ＣＤ２ ");
//				sbQuery.Append(",ST01.指定日区分 ");
//				sbQuery.Append(",ST01.郵便番号 ");
//				sbQuery.Append(",ST01.仕分ＣＤ ");
//				sbQuery.Append(",NVL(CM10.店所名, ST01.発店名) ");
//				sbQuery.Append(",ST01.出荷済ＦＧ ");
//				sbQuery.Append(",SM01.郵便番号 ");
//				sbQuery.Append(",NVL(CM01.保留印刷ＦＧ, '0') \n");
//				sbQuery.Append(",ST01.品名記事４ \n");
//				sbQuery.Append(",ST01.品名記事５ \n");
//				sbQuery.Append(",ST01.品名記事６ \n");
//				sbQuery.Append(",ST01.着店名 ");
//				sbQuery.Append(" FROM \"ＳＴ０１出荷ジャーナル\" ST01 \n");
//				sbQuery.Append(" LEFT JOIN ＣＭ０２部門 CM02 \n");
//				sbQuery.Append(" ON ST01.会員ＣＤ = CM02.会員ＣＤ \n");
//				sbQuery.Append("AND ST01.部門ＣＤ = CM02.部門ＣＤ \n");
//				sbQuery.Append(" LEFT JOIN ＣＭ１４郵便番号 CM14 \n");
//				sbQuery.Append(" ON CM02.郵便番号 = CM14.郵便番号 \n");
//				sbQuery.Append(" LEFT JOIN ＣＭ１０店所 CM10 \n");
//				sbQuery.Append(" ON CM14.店所ＣＤ = CM10.店所ＣＤ \n");
//				sbQuery.Append(" LEFT JOIN ＣＭ０１会員 CM01 \n");
//				sbQuery.Append(" ON ST01.会員ＣＤ = CM01.会員ＣＤ \n");
//				sbQuery.Append(", \"ＳＭ０１荷送人\" SM01 \n");
//				sbQuery.Append(" WHERE ST01.会員ＣＤ = '" + sKey[0] + "' \n");
//				sbQuery.Append(" AND ST01.部門ＣＤ = '" + sKey[1] + "' \n");
//				sbQuery.Append(" AND ST01.登録日 = '" + sKey[2] + "' \n");
//				sbQuery.Append(" AND ST01.ジャーナルＮＯ = '" + sKey[3] + "' \n");
//				sbQuery.Append(" AND ST01.会員ＣＤ = SM01.会員ＣＤ \n");
//				sbQuery.Append(" AND ST01.部門ＣＤ = SM01.部門ＣＤ \n");
//				sbQuery.Append(" AND ST01.荷送人ＣＤ = SM01.荷送人ＣＤ \n");
//				sbQuery.Append(" AND ST01.削除ＦＧ = '0' \n");
//				sbQuery.Append(" AND SM01.削除ＦＧ = '0' \n");

				//取得条件の設定
				sbQueryWhere.Append(" WHERE S.会員ＣＤ = '" + sKey[0] + "' \n");
				sbQueryWhere.Append(" AND S.部門ＣＤ = '" + sKey[1] + "' \n");
				sbQueryWhere.Append(" AND S.登録日 = '" + sKey[2] + "' \n");
				sbQueryWhere.Append(" AND S.\"ジャーナルＮＯ\" = '" + sKey[3] + "' \n");
				sbQueryWhere.Append(" AND S.会員ＣＤ = SM01.会員ＣＤ \n");
				sbQueryWhere.Append(" AND S.部門ＣＤ = SM01.部門ＣＤ \n");
				sbQueryWhere.Append(" AND S.荷送人ＣＤ = SM01.荷送人ＣＤ \n");
				sbQueryWhere.Append(" AND S.削除ＦＧ = '0' \n");
				sbQueryWhere.Append(" AND SM01.削除ＦＧ = '0' \n");

				//SELECT文を構築
				sbQuery.Append(GET_INVOICEPRINT_SELECT).Replace("S.", "ST01.");  //SELECT句(ST01)
				sbQuery.Append(GET_INVOICEPRINT_FROM_1);                         //FROM句(ST01)
				sbQuery.Append(sbQueryWhere).Replace("S.", "ST01.");             //WHERE句(ST01)
				sbQuery.Append(" UNION \n");
				sbQuery.Append(GET_INVOICEPRINT_SELECT).Replace("S.", "ST02.");  //SELECT句(ST02)
				sbQuery.Append(GET_INVOICEPRINT_FROM_2);                         //FROM句(ST02)
				sbQuery.Append(sbQueryWhere).Replace("S.", "ST02.");             //WHERE句(ST02)
// MOD 2016.04.08 bevas) 松本 社内伝票機能追加対応 END

				OracleDataReader reader = CmdSelect(sUser, conn2, sbQuery);
				int iCnt = 0;
				if (reader.Read())
				{
					string s輸送商品ＣＤ１ = reader.GetString(36).Trim();
					string s輸送商品ＣＤ２ = reader.GetString(37).Trim();
					sRet[1]  = reader.GetString(0).Trim();
					sRet[2]  = reader.GetString(1).Trim();
					sRet[3]  = reader.GetString(2).Trim();
					sRet[4]  = reader.GetString(3).Trim();
					sRet[5]  = reader.GetString(4).TrimEnd(); // 荷受人住所１
					sRet[6]  = reader.GetString(5).TrimEnd(); // 荷受人住所２
					sRet[7]  = reader.GetString(6).TrimEnd(); // 荷受人住所３
					sRet[8]  = reader.GetString(7).TrimEnd(); // 荷受人名前１
					sRet[9]  = reader.GetString(8).TrimEnd(); // 荷受人名前２
					sRet[10] = reader.GetString(9).Trim();
					sRet[11] = reader.GetString(10).Trim();
					sRet[12] = reader.GetString(11).Trim();
					sRet[13] = reader.GetString(12).Trim().PadLeft(4, '0');
					sRet[14] = reader.GetString(13).Trim().PadLeft(4, '0');
// MOD 2016.04.08 bevas) 松本 社内伝票機能追加対応 START
					//社内伝の場合は、出荷テーブルの方を正とする
					if(sKey[0].Substring(0,2) == "FK")
					{
						sRet[14] = reader.GetString(49).Trim().PadLeft(4, '0');
					}
// MOD 2016.04.08 bevas) 松本 社内伝票機能追加対応 END
					sRet[15] = reader.GetString(14).Trim();
					sRet[16] = reader.GetString(15).Trim();
					sRet[17] = reader.GetString(16).Trim();
					sRet[18] = reader.GetString(17).TrimEnd(); // 荷送人住所１
					sRet[19] = reader.GetString(18).TrimEnd(); // 荷送人住所２
					sRet[20] = reader.GetString(19).TrimEnd(); // 荷送人住所３
					sRet[21] = reader.GetString(20).TrimEnd(); // 荷送人名前１
					sRet[22] = reader.GetString(21).TrimEnd(); // 荷送人名前２
					sRet[23] = reader.GetDecimal(22).ToString().Trim();

					if(reader.GetString(44) == "1")
					{
						d才数 = reader.GetDecimal(33) * 8;
						if(d才数 == 0)
						{
							sRet[24] = reader.GetDecimal(23).ToString().TrimEnd();
						}
						else
						{
							sRet[24] = d才数.ToString().TrimEnd();
						}
					}
					else
					{
						sRet[24] = "";
					}

					sRet[25] = reader.GetDecimal(24).ToString().Trim();
					sRet[26] = reader.GetString(25).Trim();

					// 時間指定の場合、２行目のみ表示
					if (s輸送商品ＣＤ１.Equals("100"))
					{
						sRet[27] = reader.GetString(27).TrimEnd();
						sRet[28] = "";
					}
					// １行目と２行目が同じコードの場合、２行目を表示しない
					else if (s輸送商品ＣＤ１.Equals(s輸送商品ＣＤ２))
					{
						sRet[27] = reader.GetString(26).TrimEnd();
						sRet[28] = "";
					}
					else
					{
						sRet[27] = reader.GetString(26).TrimEnd();
						sRet[28] = reader.GetString(27).TrimEnd();
					}

					sRet[29] = reader.GetString(28).TrimEnd(); // 品名記事１
					sRet[30] = reader.GetString(29).TrimEnd(); // 品名記事２
					sRet[31] = reader.GetString(30).TrimEnd(); // 品名記事３

					// パーセルの場合、"11"
					if (s輸送商品ＣＤ１.Equals("001") || s輸送商品ＣＤ１.Equals("002"))
					{
						sRet[32] = reader.GetString(31).Trim() + "1";
					}
					else
					{
						sRet[32] = reader.GetString(31).Trim() + "0";
					}

					sRet[33] = reader.GetString(32).Trim(); // 送り状発行済ＦＧ
					sRet[34] = reader.GetString(34).TrimEnd(); // 担当者（部署）
					sRet[35] = reader.GetString(35).Trim(); // お客様番号
					sRet[36] = reader.GetString(38).Trim();
					s郵便番号 = reader.GetString(39).Trim();
					sRet[37] = reader.GetString(40).Trim();		//仕分ＣＤ
					sRet[38] = reader.GetString(41).Trim();		//発店名
// MOD 2016.04.08 bevas) 松本 社内伝票対応 START
					//社内伝の場合は、出荷テーブルの方を正とする
					if(sKey[0].Substring(0,2) == "FK")
					{
						sRet[38] = reader.GetString(50).Trim();
					}
// MOD 2016.04.08 bevas) 松本 社内伝票対応 END
					sRet[39] = reader.GetString(42).Trim();		//出荷済ＦＧ
					sRet[40] = reader.GetString(43).Trim();		//ご依頼主郵便番号
					sRet[41] = reader.GetString(44).TrimEnd();
					sRet[42] = reader.GetString(45).TrimEnd(); // 品名記事４
					sRet[43] = reader.GetString(46).TrimEnd(); // 品名記事５
					sRet[44] = reader.GetString(47).TrimEnd(); // 品名記事６
					sRet[45] = reader.GetString(48).TrimEnd(); // 着店名
					iCnt++;
				}

				disposeReader(reader);
				reader = null;

				if (iCnt == 0)
				{
					sRet[0] = "該当データがありません";
				}
				else
				{
					sRet[0] = "正常終了";
					logWriter(sUser, INF, "出荷印刷データ取得"
						+"["+sKey[1]+"]["+sKey[2]+"]["+sKey[3]+"]:["+sRet[11]+"]"
						+"送り状発行済["+sRet[33]+"]出荷済["+sRet[39]+"]"
						);
				}
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			
			return sRet;
		}

		/*********************************************************************
		 * 発店取得
		 * 引数：会員ＣＤ、部門ＣＤ
		 * 戻値：ステータス、店所ＣＤ
		 *
		 *********************************************************************/
		private static string GET_HATUTEN3_SELECT
			= "SELECT CM14.店所ＣＤ \n"
			+  " FROM ＣＭ０２部門 CM02 \n"
			+      ", ＣＭ１４郵便番号 CM14 \n"
			;

		[WebMethod]
		public String[] Get_hatuten3(string[] sUser, string sKcode, string sBcode)
		{
			logWriter(sUser, INF, "発店取得３開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[2]{"",""};

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}

			StringBuilder sbQuery = new StringBuilder(1024);
			try
			{
				sbQuery.Append(GET_HATUTEN3_SELECT);
				sbQuery.Append(" WHERE CM02.会員ＣＤ = '" + sKcode + "' \n");
				sbQuery.Append(" AND CM02.部門ＣＤ = '" + sBcode + "' \n");
				sbQuery.Append(" AND CM02.郵便番号 = CM14.郵便番号 \n");

				OracleDataReader reader = CmdSelect(sUser, conn2, sbQuery);

				if(reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();

					sRet[0] = "正常終了";
				}
				else
				{
					sRet[0] = "利用者の集荷店取得に失敗しました";
				}

				disposeReader(reader);
				reader = null;

				logWriter(sUser, INF, sRet[0]);
			}
			catch (OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch (Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			
			return sRet;
		}
// ADD 2015.11.24 bevas）松本 出荷実績表印刷機能追加(is-2管理) END
// MOD 2015.12.15 bevas) 松本 輸送禁止エリア機能対応(is-2管理：ラベルイメージ印刷時) START
		/*********************************************************************
		 * 配達不能エリアチェック
		 * 　　入力された郵便番号から、
		 * 　　ＣＭ２１配達不能の存在チェックを実施する。
		 * 引数：郵便番号
		 * 戻値：ステータス、検索文字、メッセージ、表示開始日、表示終了日
		 *********************************************************************/
		[WebMethod]
		public ArrayList Check_NonDeliveryArea(string[] sUser, string sYubinNo)
		{
			logWriter(sUser, INF, "配達不能エリアチェック開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];  //主にステータスを格納
			ArrayList alRet = new ArrayList();  //戻り値
			string cmdQuery;  // SQL文
			OracleDataReader reader;
			OracleParameter[] wk_opOraParam	= null;

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "ＤＢ接続エラー";
				alRet.Add(sRet);
				return alRet;
			}

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			try
			{
				// SQL文
				cmdQuery
					= "SELECT 検索文字, メッセージ, 表示開始日, 表示終了日 \n"
					+ "  FROM ＣＭ２１配達不能 \n"
					+ " WHERE 郵便番号 = :p_YubinNo \n"
					+ "   AND 削除ＦＧ = '0' \n"
					+ " ORDER BY 検索文字 DESC, メッセージ DESC \n";
				wk_opOraParam = new OracleParameter[1];
				wk_opOraParam[0] = new OracleParameter("p_YubinNo", OracleDbType.Char, sYubinNo, ParameterDirection.Input); // 郵便番号

				reader = CmdSelect(sUser, conn2, cmdQuery, wk_opOraParam);
				wk_opOraParam = null;

				// データ取得
				while(reader.Read())
				{
					string[] sData = new string[4];
					sData[0] = reader.GetString(0).Trim(); // 検索文字
					sData[1] = reader.GetString(1).Trim(); // メッセージ
					sData[2] = reader.GetString(2).Trim(); // 表示開始日
					sData[3] = reader.GetString(3).Trim(); // 表示終了日

					alRet.Add(sData); //リストに格納
				}

				disposeReader(reader);
				reader = null;

				if(alRet.Count == 0)
				{
					//該当データなし
					sRet[0] = "該当データなし";
					alRet.Add(sRet);
				}
				else
				{
					//該当データあり
					sRet[0] = "該当データあり";
					alRet.Insert(0, sRet);
				}
			}
			catch (OracleException ex)
			{
				// Oracle のエラー
				sRet[0] = chgDBErrMsg(sUser, ex);
				alRet.Insert(0, sRet);
			}
			catch (Exception ex)
			{
				// それ以外のエラー
				sRet[0] = "サーバエラー：" + ex.Message;
				alRet.Insert(0, sRet);
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				// 終了処理
				disconnect2(sUser, conn2);
				conn2 = null;
			}

			return alRet;
		}
// MOD 2015.12.15 bevas) 松本 輸送禁止エリア機能対応(is-2管理：ラベルイメージ印刷時) END
// MOD 2016.04.08 bevas) 松本 社内伝票機能追加対応 START
		/*********************************************************************
		 * 社内伝会員扱店マスタ一覧取得
		 * 引数：会員ＣＤ
		 * 戻値：ステータス、一覧（会員ＣＤ、会員名）...
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Get_HouseSlipMember(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "社内伝会員扱店マスタ一覧取得開始");

			OracleConnection conn2 = null;
			ArrayList sList = new ArrayList();
			string[] sRet = new string[1];

			//ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT '|' "
					+     "|| TRIM(CM05F.会員ＣＤ) || '|' "
					+     "|| NVL(CM01.会員名, ' ') || '|' "
					+     "|| TRIM(CM05F.店所ＣＤ) || '|' \n"
					+  " FROM ＣＭ０５会員扱店Ｆ CM05F \n"
					+  " LEFT JOIN ＣＭ０１会員 CM01 \n"
					+    " ON CM01.会員ＣＤ = CM05F.会員ＣＤ "
					+    "AND CM01.削除ＦＧ = '0' \n"
					+ " WHERE CM05F.削除ＦＧ = '0' \n";

				//初期表示時（入力検索条件なし）は、社内伝会員全体を取得
				if(sKey[0].Trim().Length != 0)
				{
					cmdQuery += " AND CM05F.会員ＣＤ LIKE '" + sKey[0] + "%' \n";
				}
				cmdQuery += " ORDER BY CM05F.会員ＣＤ, CM05F.店所ＣＤ \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				while(reader.Read())
				{
					sList.Add(reader.GetString(0));
				}
				disposeReader(reader);
				reader = null;

				sRet = new string[sList.Count + 1];
				if(sList.Count == 0) 
				{
					sRet[0] = "該当データがありません";
				}
				else
				{
					sRet[0] = "正常終了";
					int iCnt = 1;
					IEnumerator enumList = sList.GetEnumerator();
					while(enumList.MoveNext())
					{
						sRet[iCnt] = enumList.Current.ToString();
						iCnt++;
					}
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch(OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch(Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}

		/*********************************************************************
		 * 社内伝会員扱店マスタ取得
		 * 引数：集荷店ＣＤ
		 * 戻値：ステータス、集荷店ＣＤ、店所名...
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Sel_HouseSlipMember(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "社内伝会員扱店マスタ検索開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[6];

			//ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT CM05F.会員ＣＤ "
					+      ", NVL(CM01.会員名, ' ') "
					+      ", CM05F.店所ＣＤ "
					+      ", NVL(CM10.店所名, ' ') "
					+      ", CM05F.更新日時 \n"
					+  " FROM ＣＭ０５会員扱店Ｆ CM05F \n"
					+  " LEFT JOIN ＣＭ０１会員 CM01 \n"
					+    " ON CM01.会員ＣＤ = CM05F.会員ＣＤ "
					+    "AND CM01.削除ＦＧ = '0' \n"
					+  " LEFT JOIN ＣＭ１０店所 CM10 \n"
					+    " ON CM10.店所ＣＤ = CM05F.店所ＣＤ "
					+    "AND CM10.削除ＦＧ = '0' \n"
					+ " WHERE CM05F.会員ＣＤ = '" + sKey[0] + "' \n"
					+   " AND CM05F.削除ＦＧ   = '0' \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				while(reader.Read())
				{
					sRet[1] = reader.GetString(0).Trim();
					sRet[2] = reader.GetString(1).Trim();
					sRet[3] = reader.GetString(2).Trim();
					sRet[4] = reader.GetString(3).Trim();
					sRet[5] = reader.GetDecimal(4).ToString().Trim();
					iCnt++;
				}
				disposeReader(reader);
				reader = null;
				if(iCnt == 1)
				{
					sRet[0] = "該当データがありません";
				}
				else
				{
					sRet[0] = "正常終了";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch(OracleException ex)
			{
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch(Exception ex)
			{
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}

		/*********************************************************************
		 * 社内伝会員扱店マスタ追加
		 * 引数：集荷店ＣＤ、集約店ＣＤ...
		 * 戻値：ステータス
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Ins_HouseSlipMember(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "社内伝会員扱店マスタ追加開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];
			string s更新日時 = System.DateTime.Now.ToString("yyyyMMddHHmmss");
			string s会員ＣＤ = sKey[0];
			string s店所ＣＤ = sKey[1];
			string s利用者ＣＤ = sKey[3];

			//ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "SELECT 削除ＦＧ "
					+   "FROM ＣＭ０５会員扱店Ｆ "
					+  "WHERE 会員ＣＤ = '" + s会員ＣＤ + "' "
					+    "FOR UPDATE \n";

				OracleDataReader reader = CmdSelect(sUser, conn2, cmdQuery);
				int iCnt = 1;
				string s削除ＦＧ = "";
				while (reader.Read())
				{
					s削除ＦＧ = reader.GetString(0);
					iCnt++;
				}
				if(iCnt == 1)
				{
					//追加
					cmdQuery
						= "INSERT INTO ＣＭ０５会員扱店Ｆ \n"
						+ " VALUES ("
						+ " '" + s会員ＣＤ + "' " 
						+ ",'" + s店所ＣＤ + "' "
						+ ",'0' "
						+ "," + s更新日時
						+ ",'扱店登録' "
						+ ",'" + s利用者ＣＤ + "' "
						+ "," + s更新日時
						+ ",'社伝登録' "
						+ ",'" + s利用者ＣＤ + "' \n"
						+ " ) \n";

					CmdUpdate(sUser, conn2, cmdQuery);
					tran.Commit();
					sRet[0] = "正常終了";
				}
				else
				{
					//追加更新
					if (s削除ＦＧ.Equals("1"))
					{
						cmdQuery
							= "UPDATE ＣＭ０５会員扱店Ｆ \n"
							+   " SET 店所ＣＤ = '" + s店所ＣＤ + "' "
							+       ",削除ＦＧ = '0'"
							+       ",登録日時 = " + s更新日時
							+       ",登録ＰＧ = '扱店登録'"
							+       ",登録者 = '" + s利用者ＣＤ + "' "
							+       ",更新日時 = " + s更新日時
							+       ",更新ＰＧ = '社伝登録' "
							+       ",更新者 = '" + s利用者ＣＤ + "' \n"
							+ " WHERE 会員ＣＤ = '" + s会員ＣＤ + "' \n";

						CmdUpdate(sUser, conn2, cmdQuery);
						tran.Commit();
						sRet[0] = "正常終了";
					}
					else
					{
						tran.Rollback();
						sRet[0] = "既に登録されています";
					}
				}
				disposeReader(reader);
				reader = null;
				logWriter(sUser, INF, sRet[0]);
			}
			catch(OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch(Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}

		/*********************************************************************
		 * 社内伝会員扱店マスタ更新
		 * 引数：集荷店ＣＤ、集約店ＣＤ...
		 * 戻値：ステータス
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Upd_HouseSlipMember(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "社内伝会員扱店マスタ更新開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];
			string s更新日時 = System.DateTime.Now.ToString("yyyyMMddHHmmss");
			string sKey会員ＣＤ = sKey[0];
			string s店所ＣＤ = sKey[1];
			string sKey更新日時 = sKey[2];
			string s利用者ＣＤ = sKey[3];

			//ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "UPDATE ＣＭ０５会員扱店Ｆ \n"
					+   " SET 店所ＣＤ = '" + s店所ＣＤ + "' "
					+       ",更新日時 =  " + s更新日時
					+       ",更新ＰＧ = '扱店更新' "
					+       ",更新者 = '" + s利用者ＣＤ + "' \n"
					+ " WHERE 会員ＣＤ = '" + sKey会員ＣＤ + "' \n"
					+   " AND 削除ＦＧ = '0' \n"
					+   " AND 更新日時 = " + sKey更新日時 + " \n";

				if(CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
					tran.Commit();
					sRet[0] = "正常終了";
				}
				else
				{
					tran.Rollback();
					sRet[0] = "他の端末で更新されています";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch(OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch(Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}

		/*********************************************************************
		 * 社内伝会員扱店マスタ削除
		 * 引数：集荷店ＣＤ、更新日時、更新者
		 * 戻値：ステータス
		 *
		 *********************************************************************/
		[WebMethod]
		public string[] Del_HouseSlipMember(string[] sUser, string[] sKey)
		{
			logWriter(sUser, INF, "社内伝会員扱店マスタ削除開始");

			OracleConnection conn2 = null;
			string[] sRet = new string[1];
			string sKey会員ＣＤ = sKey[0];
			string sKey更新日時 = sKey[1];
			string s利用者ＣＤ = sKey[2];

			// ＤＢ接続
			conn2 = connect2(sUser);
			if(conn2 == null)
			{
				sRet[0] = "ＤＢ接続エラー";
				return sRet;
			}

			OracleTransaction tran;
			tran = conn2.BeginTransaction();

			string cmdQuery = "";
			try
			{
				cmdQuery
					= "UPDATE ＣＭ０５会員扱店Ｆ \n"
					+    "SET 削除ＦＧ = '1' " 
					+       ",更新日時 = TO_CHAR(SYSDATE, 'YYYYMMDDHH24MISS') "
					+       ",更新ＰＧ = '扱店削除' "
					+       ",更新者 = '" + s利用者ＣＤ + "' "
					+  "WHERE 会員ＣＤ = '" + sKey会員ＣＤ + "' "
					+    "AND 更新日時 = " + sKey更新日時 + " \n";

				if(CmdUpdate(sUser, conn2, cmdQuery) != 0)
				{
					tran.Commit();
					sRet[0] = "正常終了";
				}
				else
				{
					tran.Rollback();
					sRet[0] = "他の端末で更新されています";
				}
				logWriter(sUser, INF, sRet[0]);
			}
			catch(OracleException ex)
			{
				tran.Rollback();
				sRet[0] = chgDBErrMsg(sUser, ex);
			}
			catch(Exception ex)
			{
				tran.Rollback();
				sRet[0] = "サーバエラー：" + ex.Message;
				logWriter(sUser, ERR, sRet[0]);
			}
			finally
			{
				disconnect2(sUser, conn2);
				conn2 = null;
			}
			return sRet;
		}

// MOD 2016.04.08 bevas) 松本 社内伝票機能追加対応 END
	}
}
