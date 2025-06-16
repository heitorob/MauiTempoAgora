using MauiTempoAgora.Models;
using MauiTempoAgora.Services;
using System.Collections.ObjectModel;
using System.Globalization;

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
                lbl_res.Text = "Insira a cidade para ver a previsão.";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private async void ButtonLocalizacao_Clicked(object sender, EventArgs e)
        {
            try
            {
                GeolocationRequest request = new GeolocationRequest(
                        GeolocationAccuracy.Medium,
                        TimeSpan.FromSeconds(10)
                    );

                Location? local = await Geolocation.Default.GetLocationAsync(request);

                if (local != null)
                {
                    string local_coordenadas = $"Latitude: {local.Latitude} \n" + 
                        $"Longitude: {local.Longitude}";

                    txt_cidade.Text = await GetCidade(local.Latitude, local.Longitude);
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Erro: Dispositivo não Suporta", fnsEx.Message, "OK");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                await DisplayAlert("Erro: Localização Desabilitada", fneEx.Message, "OK");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Erro: Permissão da Localização", pEx.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async Task<string> GetCidade(double lat, double lon)
        {
            try
            {
                IEnumerable<Placemark> places = await Geocoding.Default.GetPlacemarksAsync(lat, lon);

                Placemark? place = places.FirstOrDefault();

                if (place != null)
                    return place.Locality;
                else
                    return "";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro: Obtenção do nome da Cidade", ex.Message, "OK");
                return "";
            }
        }

        private async void ButtonPrevisao_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null)
                    {
                        t.cidade = (await GetCidade(t.lat, t.lon)) ?? "Cidade não identificada";
                        t.data = DateTime.Now;
                        string dados_previsao = "";

                        dados_previsao = $"Latitude: {t.lat} \n" +
                                         $"Longitude: {t.lon} \n" +
                                         $"Nascer do Sol: {t.sunrise} \n" +
                                         $"Pôr do Sol: {t.sunset} \n" +
                                         $"Temperatura Máxima: {t.tempmax} \n" +
                                         $"Temperatura Mínima: {t.tempmin} \n" +
                                         $"Descrição: {t.description}";

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
                txt_cidade.Text = null;
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
