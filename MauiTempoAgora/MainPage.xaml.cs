using MauiTempoAgora.Models;
using MauiTempoAgora.Services;
using System.Collections.ObjectModel;

namespace MauiTempoAgora
{
    public partial class MainPage : ContentPage
    {
        ObservableCollection<Tempo> lista = new ObservableCollection<Tempo>();

        public MainPage()
        {
            InitializeComponent();

            lst_historico.ItemsSource = lista;
        }

        protected async override void OnAppearing()
        {
            try
            {
                lista.Clear();

                List<Tempo> tmp = await App.Db.GetAll();

                tmp.ForEach(i => lista.Add(i));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private async void ButtonPrevisao_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);
                    t.cidade = txt_cidade.Text;

                    if (t != null)
                    {
                        string dados_previsao = "";

                        dados_previsao = $"Latitude: {t.lat} \n" +
                                         $"Longitude: {t.lon} \n" +
                                         $"Nascer do Sol: {t.sunrise} \n" +
                                         $"Por do Sol: {t.sunset} \n" +
                                         $"Temperatura Máxima: {t.tempmax} \n" +
                                         $"Temperatura Mínima: {t.tempmin} \n";

                        lbl_res.Text = dados_previsao;

                        try
                        {
                            await App.Db.Insert(t);
                            lista.Add(t);
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("Erro", ex.Message, "OK");
                        }
                    }
                    else
                    {
                        lbl_res.Text = "Sem dados de previsão.";
                    }
                }
                else
                {
                    lbl_res.Text = "Preencha a cidade.";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string q = e.NewTextValue;

                lst_historico.IsRefreshing = true;

                lista.Clear();

                List<Tempo> tmp = await App.Db.Search(q);

                tmp.ForEach(i => lista.Add(i));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
            finally
            {
                lst_historico.IsRefreshing = false;
            }
        }

        private async void lst_historico_Refreshing(object sender, EventArgs e)
        {
            try
            {
                lista.Clear();

                List<Tempo> tmp = await App.Db.GetAll();

                tmp.ForEach(i => lista.Add(i));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
            finally
            {
                lst_historico.IsRefreshing = false;
            }
        }

        private async void lst_historico_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                Tempo t = e.SelectedItem as Tempo;

                if (t == null)
                {
                    await DisplayAlert("Erro", "Nenhum registro selecionado.", "OK");
                    return;
                }

                bool resposta = await DisplayAlert("Apagar Registro", "Deseja realmente apagar este registro?", "Sim", "Não");
                if (!resposta) return;

                await App.Db.Delete(t.Id);

                try
                {
                    lista.Clear();
                    List<Tempo> tmp = await App.Db.GetAll();
                    tmp.ForEach(i => lista.Add(i));
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Ops", ex.Message, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }
}
